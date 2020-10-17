'use strict';

function listCats(showList) {
    $.ajax({
        type: "POST",
        traditional: true,
        async: true,
        cache: false,
        url: "listCats/",
        context: document.body,
        beforeSend: function (result) {
            $body.addClass("loading");
        },
        success: function (result) {
            petgree.listCats = JSON.parse(result);
            //sessionStorage.setItem('petgreelist', JSON.stringify(petgree));
            petData.updateData(petgree)
            if (showList)
                showListCats();

            $body.removeClass("loading");
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    });
}

function showListCats() {

    //$('#table_listCats').DataTable().destroy();
    //$('#table_listCats').DataTable({
    //    "searching": true,
    //    "retrieve": true
    //});
    //$('#table_listCats').empty();

    //$('#table_listCats').DataTable().destroy();
    //$('#table_listCats').DataTable({
    //    "searching": true,
    //    "retrieve": true
    //});



    petgree = petData.get();    //JSON.parse(sessionStorage.getItem("petgreelist"));
    for (var i = 0; i < petgree.listCats.length; i++) {
        var trString = '<tr class="odd gradeX" onclick="viewCatsProfile(' + petgree.listCats[i].id + ')" style="cursor: pointer;" >';
        trString += "<td>" + petgree.listCats[i].id + "</td>";
        trString += "<td>" + petgree.listCats[i].homologation + "</td>";
        trString += "<td>" + padl(moment(petgree.listCats[i].arrival_date).date(), 2) + "/" +
            padl(moment(petgree.listCats[i].arrival_date).month() + 1, 2) + "/" +
            moment(petgree.listCats[i].arrival_date).year(); + "</td>";

        trString += "<td>" + petgree.listCats[i].name + "</td>";

        trString += "<td>" + padl(moment(petgree.listCats[i].birth_date).date(), 2) + "/" +
            padl(moment(petgree.listCats[i].birth_date).month() + 1, 2) + "/" +
            moment(petgree.listCats[i].birth_date).year(); + "</td>";

        trString += "<td>" + petgree.listCats[i].race.description + "</td>";
        trString += "<td>" + petgree.listCats[i].color.description + "</td>";
        trString += "</tr>";
        $('#table_listCats').append(trString);
    };

    //$('#table_listCats').DataTable({
    //    "searching": true,
    //    "retrieve": true
    //});

    //$('#table_listCats').DataTable({
    //    "searching": true
    //});



    $('#table_listCats').DataTable({
        'processing': true,
        'language': {
            'loadingRecords': '&nbsp;',
            'processing': 'Loading...',
            "searching": true
        }
    });


}

function viewCatsProfile(id) {
    petgree = petData.get(); 
    var cat = petgree.listCats.find(x => x.id = id)
    $.ajax({
        type: "POST",
        traditional: true,
        async: true,
        cache: false,
        url: "getCat/" + cat.id,
        context: document.body,
        beforeSend: function (result) {
            $body.addClass("loading");
        },
        success: function (result) {
            var data = Object.assign(new Cat(), result)
            loadcat(data);
            $('#catsProfile').modal('show');
            $body.removeClass("loading");
        },
        error: function (xhr) {
            alert("Error has occurred while getting settings");
        }
    });


}


function listProperties(key) {
    $.ajax({
        type: "POST",
        traditional: true,
        async: true,
        cache: false,
        url: "listProperties?key=" + key,
        context: document.body,
        beforeSend: function (result) {
            //$body.addClass("loading");
        },
        success: function (result) {
            if (key == "Color")
                petgree.listColors = result;
            if (key == "Mark")
                petgree.listMarks = result;
            if (key == "Particolor")
                petgree.listPartiColor = result;
            if (key == "Pather")
                petgree.listPatter = result;
            if (key == "Eyes")
                petgree.listEyes = result;

            petData.updateData(petgree);
            //sessionStorage.setItem('petgreelist', JSON.stringify(petgree));
            //$body.removeClass("loading");
        },
        error: function (xhr) {
            alert("Error has occurred while getting properties");
        }
    });

}


function listraces() {
    $.ajax({
        type: "POST",
        traditional: true,
        async: true,
        cache: false,
        url: "listRace/",
        context: document.body,
        beforeSend: function (result) {
            //$('#overlay').fadeIn();
        },
        success: function (result) {
            petgree.listRaces = result;
            //sessionStorage.setItem('petgreelist', JSON.stringify(petgree));
            petData.updateData(petgree)
            //$('#overlay').fadeOut();

        },
        error: function (xhr) {
            alert("Error has occurred while getting settings");
        }
    });

}


function salveCat() {
    var cat = new Cat($("#id").val());

    cat.name = $("#name").val();
    cat.birth_date = $("#birth").datepicker("getDate");
    cat.arrival_date = $("#arrivel").datepicker("getDate");

    var race = new cat_general_sigla();
    race.id = $("#race").val();
    race.description = $("#race option:selected").text()
    cat.race = race;

    var father = new parent();
    father.id = $("#father_id").val();
    father.name = $("#father").val();

    var fatherRace = new cat_general_sigla();
    fatherRace.id = $("#father_race_id").val()
    fatherRace.description = $("#father_race").val()
    father.race = fatherRace

    cat.father = father;

    var mother = new parent();
    mother.id = $("#mother_id").val();
    mother.name = $("#mother").val();

    var motherRace = new cat_general_sigla();
    motherRace.id = $("#mother_race_id").val();
    motherRace.name = $("#mother_race").val();
    mother.race = motherRace;
    cat.mother = mother;

    var color = new cat_general();
    color.id = $("#color_id").val();
    color.description = $("#color").val();
    cat.color = color;

    var marks = new cat_general();
    marks.id = $("#mark_id").val();
    marks.description = $("#mark").val();
    cat.marks = marks;

    var particolor = new cat_general();
    particolor.id = $("#particolor_id").val();
    particolor.description = $("#particolor").val();
    cat.partcolor = particolor;

    var patter = new cat_general();
    patter.id = $("#pather_id").val();
    patter.description = $("#pather").val();
    cat.pather = patter;

    cat.sex = $("#sex").val();

    var eyes = new cat_general_sigla();
    eyes.id = $("#eyes_id").val();
    eyes.description = $("#eyes").val();
    eyes.sigla = $("#eyes_sigla").val();
    cat.eye = eyes;

    cat.country_origin = $("#country").val();
    cat.microship = $("#microship").val();
    cat.breeder = $("#breeder").val();
    cat.homologation = $("#pedigree").val();

    var cattery = new Person();
    cattery.id = $("#cattery_id").val();
    cat.cattery = cattery;

    var creator = new Person();
    creator.id = $("#creator_id").val();
    cat.creator = creator;

    var proprietary = new Person();
    proprietary.id = $("#proprietary_id").val();
    cat.proprietary = proprietary;

    cat.document = new documents()
    cat.obs = $("#obs").val();

    cat.action = (cat.id <= 0 ? "INSERT" : "EDIT");

    $.ajax({
        type: "POST",
        url: "saveCat/",
        data: JSON.stringify(cat),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (result) {
            //alert(result);
            //showSpinner();
        },
        success: function (result) {
            //hideSpinner();
            //loadcat(result);
            petData.updateCats();
            $('#catsProfile').modal('toggle');
        },
        error: function (xhr) {
            alert("Error has occurred while getting settings");
        }
    });


}


function loadcat(data) {

    $("#id").val(data.id);
    $("#name").val(data.name);

    fillRacesDropdown(data);

    var birth = padl(moment(data.birth_date).date(), 2) + "/" +
        padl(moment(data.birth_date).month() + 1, 2) + "/" +
        moment(data.birth_date).year();
    $("#birth_input").val(birth);
    $('#birth').datepicker('setDate', birth);

    var arrivel = padl(moment(data.arrival_date).date(), 2) + "/" +
        padl(moment(data.arrival_date).month() + 1, 2) + "-" +
        moment(data.arrival_date).year();

    $("#arrivel_input").val(arrivel);
    $("#arrivel").datepicker('setDate', arrivel);

    $("#pedigree").val(data.homologation);
    $("#microship").val(data.microship);

    $("#father_id").val(data.father.id);
    $("#father").val(data.father.name);
    $("#father_race").val(data.father.race.description);
    $("#father_race_id").val(data.father.race.id);


    $("#mother_id").val(data.mother.id);
    $("#mother").val(data.mother.name);
    $("#mother_race").val(data.mother.race.description);
    $("#mother_race_id").val(data.mother.race.id);

    $("#color_id").val(data.color.id);
    $("#color").val(data.color.description);

    $("#mark_id").val(data.marks.id);
    $("#mark").val(data.marks.description);

    $("#particolor_id").val(data.partcolor.id);
    $("#particolor").val(data.partcolor.description);

    $("#pather_id").val(data.pather.id);
    $("#pather").val(data.pather.description);

    $("#sex").val(data.sex == "M" ? "Male" : "Female");

    $("#eyes_id").val(data.eye.id);
    $("#eyes").val(data.eye.description);
    $("#eyes_sigla").val(data.eye.sigla);

    $("#breeder").val(data.breeder);

    $("#country").val(data.country_origin);

    if (data.cattery != null) {
        $("#cattery_id").val(data.cattery.id);
        $("#cattery").val(data.cattery.name);
    }

    if (data.creator != null) {
        $("#creator_id").val(data.creator.id);
        $("#creator").val(data.creator.name);
    }

    if (data.proprietary != null) {
        $("#proprietary_id").val(data.proprietary.id);
        $("#proprietary").val(data.proprietary.name);
    }

    $("#obs").val(data.obs);

    if (data.id <= 0)
        $('#catsProfile').modal('show');

}


function searchCat(key) {
    $body.addClass("loading");
    cat_selected = key;
    petgree = petData.get();
    searchCatWindow(petgree.listCats);
    $body.removeClass("loading");
}

function searchCatWindow(data) {
    if ($('#table_cat_search tr').length < 2) {
        for (var i = 0; i < data.length; i++) {
            var trString = '<tr class="odd gradeX" onclick="selectCat(' + data[i]["id"] + ",'" +
                data[i]["name"].replace("'", "`") + "','" +
                data[i]["race"]["description"].replace("'", "`") + "','" +
                data[i]["race"]["id"] + "'" + ')" >';


            trString += "<td>" + data[i]["id"] + "</td>";
            trString += "<td>" + data[i]["homologation"] + "</td>";
            trString += "<td>" + data[i]["name"] + "</td>";
            trString += "<td>" + data[i]["race"]["description"] + "</td>";
            trString += "<td>" + data[i]["color"]["description"] + "</td>";
            trString += "</tr>";
            $('#table_cat_search').append(trString);
        }

        $('#table_cat_search').DataTable({
            "searching": true
        });

    }
    $('#cat_search').modal('toggle');
}


function selectCat(id, name, race, race_id) {
    $('#' + cat_selected + "_id").val(id);
    $('#' + cat_selected).val(name);
    $('#' + cat_selected + '_race').val(race);
    $('#' + cat_selected + '_race_id').val(race_id);

    $('#cat_search').modal('toggle');
}



function fillRacesDropdown(data) {

    var $cat_race = $("#race");
    var $father_race = $("#father_race");
    var $mother_race = $("#mother_race");

    for (var i = 0; i < petgree.listRaces.length; i++) {
        $cat_race.append($("<option/>").val(petgree.listRaces[i]["id"]).text(petgree.listRaces[i]["description"]));
        $father_race.append($("<option/>").val(petgree.listRaces[i]["id"]).text(petgree.listRaces[i]["description"]));
        $mother_race.append($("<option/>").val(petgree.listRaces[i]["id"]).text(petgree.listRaces[i]["description"]));
    }
    $("#race").val(data.race.id);

}


function searchPropertie(key) {
    var data;
    petgree = petData.get(); //JSON.parse(sessionStorage.getItem("petgreelist"))

    if (key == "Color")
        data = petgree.listColors;

    if (key == "Mark")
        data = petgree.listMarks;

    if (key == "Particolor")
        data = petgree.listPartiColor;

    if (key == "Pather")
        data = petgree.listPatter;

    if (key == "Eyes")
        data = petgree.listEyes;

    loadprops(data, key);


}

function loadprops(data, key) {
    //$('#table_prop_search').DataTable().destroy();
    //$('#table_prop_search').DataTable({
    //    'processing': true,
    //    'language': {
    //        'loadingRecords': '&nbsp;',
    //        'processing': 'Loading...'
    //    }
    //});
    //$('#table_prop_search').empty();

    for (var i = 0; i < data.length; i++) {
        var trString = '<tr class="odd gradeX" onclick="selectProp(' + data[i]["id"] + ",'" +
            data[i]["description"].replace("'", "`") + "','" +
            data[i]["sigla"].replace("'", "`") + "','" + key + "'" + ')" style="cursor: pointer;" >';


        trString += "<td>" + data[i]["id"] + "</td>";
        trString += "<td>" + data[i]["description"] + "</td>";
        trString += "<td>" + data[i]["sigla"] + "</td>";

        trString += "<th style=\"text-align: center;\">";

        //trString += "<div class=\"row\">";
        //trString += "<div class=\"col-sm-3\">";
        trString += "<button type=\"button\" class=\"btn btn-info btn-circle\" onclick=\"editProp('" + key + "', " + data[i]["id"] + ",'" + data[i]["description"].replace("'", "`") + "','" + data[i]["sigla"].replace("'", "`") + "')\" >";
        trString += "<i class=\"fa fa-pencil\"></i>";
        trString += "</button>";
        //trString += "</div>";
        //trString += "<div class=\"col-sm-3\">";
        trString += "<button type=\"button\" class=\"btn btn-danger btn-circle\" onclick=\"delProp('" + key + "', " + data[i]["id"] + ",'" + data[i]["description"].replace("'", "`") + "' )\" >";
        trString += "<i class=\"fa fa-times\"></i>";
        trString += "</button>";
        //trString += "</div>";
        //trString += "</div>";
        trString += "</th>";
        trString += "</tr>";
        $('#table_prop_search').append(trString);
    }
    $('#table_prop_search').DataTable().draw();

    //$('#table_prop_search').DataTable({
    //    'processing': true,
    //    'language': {
    //        'loadingRecords': '&nbsp;',
    //        'processing': 'Loading...'
    //    }
    //});

    //$('#table_prop_search').DataTable({
    //    'processing': true,
    //    'language': {
    //        'loadingRecords': '&nbsp;',
    //        'processing': 'Loading...'
    //    },
    //    "searching": true
    //});


    $("#prop_tit").text(key);
    $('#prop_search').modal('toggle');
    $("#prop_tit_edit").text(key);

    //$('#table_prop_search').DataTable().destroy();
    //$('#table_prop_search').DataTable().reload

}

function selectProp(id, description, sigla, key) {
    $("#" + key.toLowerCase() + "_id").val(id);
    $("#" + key.toLowerCase()).val(description);
    $('#prop_search').modal('toggle');
}

function editProp(key, id, description, sigla) {
    if (id != null) {
        $("#prop_id").val(id);
        $("#prop_description").val(description);
        $("#prop_sigla").val(sigla);
    } else {
        $("#prop_id").val(0);
        $("#prop_description").val("");
        $("#prop_sigla").val("");
    }

    $("#prop_save").attr("onclick", "saveProp( " + (id != null) + " )");

    $('#prop_edit').modal('toggle');
    event.stopPropagation();
}

function delProp(key, id, description) {
    $('#msg_yes_no').modal('toggle');
    $("#message_key").val(key);
    $("#message_id").val(id);

    $("#btn_yes").attr("onclick", "deleteProp('" + key + "'," + id + ")");

    $("#yes_no_text").text("Confirme the exclusion of " + description + "?");
    event.stopPropagation();
}

function deleteProp(key, id) {
    $.ajax({
        type: "POST",
        traditional: true,
        async: true,
        cache: false,
        url: "delProperties?key=" + key + "&id=" + id,
        context: document.body,
        beforeSend: function (result) {
            //$('#overlay').fadeIn();
        },
        success: function (result) {
            if (result == false) {
                alert("This propertie is in use and can not be excluded.");
            }
            $('#msg_yes_no').modal('toggle');

            $("#prop_tit").text("");
            //$('#prop_search').modal('toggle');
            petData.updateProp(key);
            searchPropertie(key);
            //$('#overlay').fadeOut();
        },
        error: function (xhr) {
            alert("Error has occurred deleting propertie");
        }
    });

}

function saveProp(edit) {
    var key = $("#prop_tit_edit").text();
    $.ajax({
        type: "POST",
        traditional: true,
        async: true,
        cache: false,
        url: "saveProperties?key=" + key + "&id=" + $("#prop_id").val() + "&description=" + $("#prop_description").val() + "&sigla=" + $("#prop_sigla").val() + "&edit=" + edit,
        context: document.body,
        beforeSend: function (result) {
            //$('#overlay').fadeIn();
        },
        success: function (result) {
            $("#prop_tit").text("");
            petData.updateProp(key);
            searchPropertie(key);
            $('#prop_edit').modal('toggle');
            //$('#overlay').fadeOut();
            //$('#prop_search').modal('toggle');
        },
        error: function (xhr) {
            alert("Error has occurred deleting propertie");
        }
    });
}



function pedigree()
{
    var id = $("#id").val();
    window.open("/en/Pedigree?id=" + id, '_blank');
    /*
    $.ajax({
        type: "POST",
        traditional: true,
        async: true,
        cache: false,
        url: "pedigree?id=" + id,
        context: document.body,
        beforeSend: function (result) {
        },
        success: function (result) {
            //console.log(result)
            var data = Object.assign(new Pedigree(), result)
            petgreeData = petData.get();
            petgreeData.lastPedigree = data;
            petData.updateData(petgreeData);
            window.open("/en/Pedigree", '_blank');
            
        },
        error: function (xhr) {
            alert("Error has occurred getting pedigree");
        }
    });
    */

}

