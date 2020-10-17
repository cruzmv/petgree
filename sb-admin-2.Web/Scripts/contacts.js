'use strict';


function listPeople(showList) {
    $.ajax({
        type: "POST",
        traditional: true,
        async: true,
        cache: false,
        url: "listContacts/",
        context: document.body,
        beforeSend: function (result) {
            //$body.addClass("loading");
        },
        success: function (result) {
            petgree.listPeople = result;
            petData.updateData(petgree)
            if (showList)
                showListContact();
        },
        error: function (xhr) {
            alert("Error has occurred while getting settings");
        }
    });

}


function searchContact(key) {
    contact_selected = key;
    petgree = petData.get();    //JSON.parse(sessionStorage.getItem("petgreelist"))
    searchContactWindow(petgree.listPeople);

}


function searchContactWindow(data) {
    if ($('#table_contact_search >tbody >tr').length <= 0) {
        for (var i = 0; i < data.length; i++) {
            if (parseInt(data[i]["id"]) > 0)
            {
                var trString = '<tr class="odd gradeX" onclick="selectContact(' + data[i]["id"] + ",'" +
                    data[i]["name"].replace("'", "`") + "'" + ' )" style="cursor: pointer;" >';

                trString += "<td>" + data[i]["id"] + "</td>";
                trString += "<td>" + data[i]["name"] + "</td>";
                trString += "<td>" + data[i]["nick"] + "</td>";
                trString += "<td>" + data[i]["cnpj"] + "</td>";
                trString += "<td>" + data[i]["ie"] + "</td>";
                trString += "<td>" + data[i]["caterry_register"] + "</td>";
                trString += "<td>" + data[i]["city"] + "</td>";
                trString += "<td>" + data[i]["region"] + "</td>";
                trString += "</tr>";
                $('#table_contact_search').append(trString);
            }
        }

        $('#table_contact_search').DataTable({
            "searching": true
        });
    }


    $('#contact_search').modal('toggle');
}


function showListContact() {

    petgree = petData.get();   ///JSON.parse(sessionStorage.getItem("petgreelist"));
    var data = petgree.listPeople;
    $('#table_list_people >tbody >tr').empty()
    for (var i = 0; i < data.length; i++) {
        if (parseInt(data[i]["id"]) > 0)
        {
            var trString = '<tr class="odd gradeX" onclick="selectPerson(' + data[i]["id"] + ",'" +
                data[i]["name"].replace("'", "`") + "'" + ')" style="cursor: pointer;" >';

            trString += "<td>" + data[i]["id"] + "</td>";
            trString += "<td>" + data[i]["name"] + "</td>";
            trString += "<td>" + data[i]["nick"] + "</td>";
            trString += "<td>" + data[i]["cnpj"] + "</td>";
            trString += "<td>" + data[i]["ie"] + "</td>";
            trString += "<td>" + data[i]["caterry_register"] + "</td>";
            trString += "<td>" + data[i]["city"] + "</td>";
            trString += "<td>" + data[i]["region"] + "</td>";
            trString += "</tr>";
            $('#table_list_people').append(trString);
        }
    }

}


function selectContact(id, name)
{
    $("#" + contact_selected + "_id").val(id);
    $("#" + contact_selected).val(name);
    $('#contact_search').modal('toggle');
}



function selectPerson(id, name)
{

    if (parseInt(id) > 0) {
        $.ajax({
            type: "POST",
            traditional: true,
            async: true,
            cache: false,
            url: "peopleProfile?id=" + id,
            context: document.body,
            beforeSend: function (result) {
                //$body.addClass("loading");
            },
            success: function (result) {
                var data = Object.assign(new Person(), result)
                personProfile(data);
            },
            error: function (xhr) {
                alert("Error has occurred while getting settings");
            }
        });
    } else {

        personProfile(null);
    }

}

function personProfile(data)
{
    petgree = petData.get();
    if (data != null) {
        

        for (var i = 0; i <= petgree.listPeople.length; i++) {
            if (petgree.listPeople[i].id === data.id) {
                petgree.listPeople[i] = data;
                break;
            }
        }
        
    } else {
        data = new Person(0);
        petgree.listPeople.push(data)
    }
    petData.updateData(petgree);


    $("#id").val(data.id);
    $("#name").val(data.name);
    

    if (data.type === "F") {
        $("#lbFisica").addClass("active");
        $("#lbJuridica").removeClass("active")
    } else {
        $("#lbFisica").remove("active");
        $("#lbJuridica").addClass("active");
    }

    if (data.sex === "F") {
        $("#lbMale").removeClass("active");
        $("#lbFemale").addClass("active");
    } else {
        $("#lbMale").addClass("active");
        $("#lbFemale").removeClass("active");
    }


    $("#nickname").val(data.nick);
    $("#cattery").val(data.caterry);
    $("#cattery_register").val(data.caterry_register);
    $("#cpf").val(data.cnpj);
    $("#ie").val(data.ie);


    var birth = padl(moment(data.birthday).date(), 2) + "/" +
        padl(moment(data.birthday).month() + 1, 2) + "/" +
        moment(data.birthday).year();
    $("#birth_input").val(birth);
    $('#birth').datepicker('setDate', birth);

    var register = padl(moment(data.registered).date(), 2) + "/" +
        padl(moment(data.registered).month() + 1, 2) + "/" +
        moment(data.registered).year();
    $("#register_input").val(register);
    $('#register').datepicker('setDate', register);

    $("#street").val(data.street);
    $("#number").val(data.door_number);
    $("#district").val(data.district);
    $("#complement").val(data.complement);
    $("#city").val(data.city);
    $("#state").val(data.region);
    $("#post-code").val(data.post_code);
    $("#country").val(data.country);
    $("#obs").val(data.obs);

    updateContactsGrid();

    $('#contactsProfile').modal('toggle');
}


function selectContact(id, type,contact)
{
    $("#contact_id").val(0);
    $("#contact_type").val("");
    $("#contact_contact").val("");

    if (parseInt(id)>0)
    {
        $("#contact_id").val(id)
        $("#contact_type").val(type)
        $("#contact_contact").val(contact)
    }
    $('#contact_edit').modal('toggle');
}


function saveContactsContac()
{
    var id = $("#contact_id").val();
    var type = $("#contact_type").val();
    var contact = $("#contact_contact").val();
    var person_id = $("#id").val()

    if (type && contact) {
        petgree = petData.get();
        var person = petgree.listPeople.find(x => x.id == person_id);
        var personIndex = petgree.listPeople.findIndex(x => x.id == person_id);

        if (parseInt(id)) {
            var contact = { id: parseInt(id), type: type, contacts_contact: contact };
            var index = person.contact.findIndex(x => x.id == id);
            person.contact[index] = contact;
        } else {
            id = 1;
            try {
                id = (person.contact.sort((a, b) => a - b).reverse()[0]).id;
            } catch (ex) { }

            if (person.contact[0] === undefined)
            {
                person.contact = [];
            }

            person.contact.push({ id: id + 1, type: type, contacts_contact: contact })
        }

        petgree.listPeople[personIndex] = person;
        petData.updateData(petgree);

        updateContactsGrid();

        $('#contact_edit').modal('toggle');
    } else {
        alert("Please inform the type and contact.");
    }

}


function updateContactsGrid()
{
    var person_id = $("#id").val()
    $('#table_person_contact >tbody >tr').empty()
    petgree = petData.get();
    var person = petgree.listPeople.find(x => x.id == person_id);        
    for (var i = 0; i <= person.contact.length - 1; i++) {
        var trString = '<tr class="odd gradeX" onclick="selectContact(' + person.contact[i].id + ",'" + person.contact[i].type + "','" + person.contact[i].contacts_contact + "'" + ' )" style="cursor: pointer;" >';
        trString += "<td>" + person.contact[i].type + "</td>";
        trString += "<td>" + person.contact[i].contacts_contact + "</td>";
        trString += "<td><button type=\"button\" class=\"btn btn-danger btn-circle\" onclick=\"delContact(" + person.contact[i].id + ")\" >";
        trString += "<i class=\"fa fa-times\"></i>";
        trString += "</button></td>";
        trString += "</tr>";
        $('#table_person_contact').append(trString);
    }
}


function delContact(id)
{

    petgree = petData.get();
    var person_id = $("#id").val()
    var person = petgree.listPeople.find(x => x.id == person_id);
    var contact = person.contact.find(x => x.id == id);


    $('#msg_yes_no').modal('toggle');
    $("#message_key").val("contact");
    $("#message_id").val(id);

    $("#btn_yes").attr("onclick", "deleteContact(" + id + ")");

    $("#yes_no_text").text("Confirme the exclusion of " + contact.contacts_contact + "?");
    event.stopPropagation();
}

function deleteContact(id)
{
    petgree = petData.get();
    var person_id = $("#id").val()
    var person = petgree.listPeople.find(x => x.id == person_id);
    var personIndex = petgree.listPeople.findIndex(x => x.id == person_id);
    //var contact = person.contact.find(x => x.id == id);
    var contactIndex = person.contact.findIndex(x => x.id == id);
    person.contact.splice(contactIndex)
    petgree.listPeople[personIndex] = person;
    petData.updateData(petgree);
    updateContactsGrid();
    $('#msg_yes_no').modal('toggle');
}


function salveContact()
{
    petgree = petData.get();
    var person_id = $("#id").val()
    var person = petgree.listPeople.find(x => x.id == person_id);
    var personIndex = petgree.listPeople.findIndex(x => x.id == person_id);

    person.name = $("#name").val();
    person.type = ($("#lbFisica").hasClass("active") ? "F" : "J");
    person.sex = ($("#lbMale").removeClass("active") ? "M" : "F");
    person.nick = $("#nickname").val();
    person.caterry = $("#cattery").val();
    person.caterry_register = $("#cattery_register").val();
    person.cnpj =  $("#cpf").val();
    person.ie = $("#ie").val();
    person.birthday = $("#birth").datepicker("getDate")
    person.registered = $("#register").datepicker("getDate")
    person.street = $("#street").val();
    person.door_number = $("#number").val();
    person.district = $("#district").val();
    person.complement = $("#complement").val();
    person.city = $("#city").val();
    person.region = $("#state").val();
    person.post_code = $("#post-code").val();
    person.country = $("#country").val();
    person.obs = $("#obs").val();

    petgree.listPeople[personIndex] = person;


    $.ajax({
        type: "POST",
        url: "savePerson/",
        data: JSON.stringify(person),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (result) {
        },
        success: function (result) {
            if (result == true) {
                //showListContact();
                $('#contactsProfile').modal('toggle');
            }
        },
        error: function (xhr) {
            alert("Error has occurred while saving contact");
        }
    });


}