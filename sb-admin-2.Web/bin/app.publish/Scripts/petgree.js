'use strict';

var petgreeData = {
    "listCats": null,
    "listPeople": null,
    "listColors": null,
    "listMarks": null,
    "listPartiColor": null,
    "listPatter": null,
    "listEyes": null,
    "listCountry": null,
    "listRaces": null,
    "lastPedigree": null
}


class cat_general_sigla {
    id = 0;
    description = "";
    sigla = "";

}


class cat_general {
    id = 0;
    description = "";
}

class parent {
    id = 0;
    name = "";
    race = new cat_general_sigla();
}

class documents {
    id = 0;
    date = null;
    description = "";
    file = "";
}

class Person {
    id = 0;
    name = "";
    type = "";
    sex = "";
    nick = "";
    caterry = "";
    cnpj = "";
    ie = "";
    birthday = null;
    registered = null;
    caterry_register = "";
    street = "";
    door_number = 0;
    district = "";
    complement = "";
    city = "";
    region = "";
    post_code = "";
    country = "";
    contact = new contact();
    attached = new attached();
    obs = "";

}


class contact
{
    id = 0;
    type = "";
    contacts_contact = "";
    obs = "";
}

class attached
{
    id = 0;
    file = null;
}


class Cat {
    id = 0;
    name = "";
    birth_date = null;
    arrival_date = null;
    value_payd = 0;
    race = new cat_general_sigla();
    father = new parent();
    mother = new parent();
    color = new cat_general();
    marks = new cat_general();
    partcolor = new cat_general();
    pather = new cat_general();
    sex = "";
    eye = new cat_general_sigla();
    country_origin = "";
    microship = "";
    breeder = "";
    homologation = "";
    cattery = new Person;
    creator = new Person;
    proprietary = new Person;
    document = new documents()
    obs = "";
    action = "";
    constructor(_id) {
        this.id = _id;
    }



}




class Pedigree
{
    // Cat
    cat = new Cat();

            // Pai 
            pai  = new Cat();
                // Avôs paternos
                pai_avo  = new Cat();
                    pai_avo_bisavo  = new Cat();
                        pai_avo_bisavo_tataravo = new Cat();
                        pai_avo_bisavo_tataravoo  = new Cat();

                    pai_avo_bisavoo  = new Cat();
                        pai_avo_bisavoo_tataravo  = new Cat();
                        pai_avo_bisavoo_tataravoo  = new Cat();

                // Avós paterno
                pai_avoo  = new Cat();
                    pai_avoo_bisavo  = new Cat();
                        pai_avoo_bisavo_tataravo  = new Cat();
                        pai_avoo_bisavo_tataravoo  = new Cat();

                    pai_avoo_bisavoo  = new Cat();
                        pai_avoo_bisavoo_tataravo  = new Cat();
                        pai_avoo_bisavoo_tataravoo  = new Cat();



            // Mae
            mae = new Cat();


                // Avôs Maternos
                mae_avo  = new Cat();
                    mae_avo_bisavo  = new Cat();
                        mae_avo_bisavo_tataravo  = new Cat();
                        mae_avo_bisavo_tataravoo  = new Cat();

                    mae_avo_bisavoo  = new Cat();
                        mae_avo_bisavoo_tataravo  = new Cat();
                        mae_avo_bisavoo_tataravoo  = new Cat();

                // Avós Maternos
                mae_avoo  = new Cat();
                    mae_avoo_bisavo  = new Cat();
                        mae_avoo_bisavo_tataravo  = new Cat();
                        mae_avoo_bisavo_tataravoo  = new Cat();

                    mae_avoo_bisavoo  = new Cat();
                        mae_avoo_bisavoo_tataravo  = new Cat();
                        mae_avoo_bisavoo_tataravoo  = new Cat();

}




function padl(n, width, z) {
    z = z || '0';
    n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
}



function msg_yes()
{
    return false;
}


const petData = {
    data: JSON.parse(sessionStorage.getItem("petgreelist")),
    updateCats() {
        listCats(false);
    },
    updateProp(key) {
        listProperties(key);
    },
    updateData(result) {
        sessionStorage.setItem('petgreelist', JSON.stringify(result));
    },
    get() {
        return JSON.parse(sessionStorage.getItem("petgreelist"));
    }
};

//sessionStorage.setItem('petgreelist', JSON.stringify(petgree));
//petData
//collection.add('C', 'Java', 'PHP');
//collection.get(1) // => 'Java'