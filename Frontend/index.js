$(function () {
    var kw = 22;
    $('<div id="mainDiv">').appendTo('body');
    $('<button type="button" id="btn_Persons" class="btn btn-primary">Persons</button>').on('click', event => {
        showPersons();
    }).appendTo('#mainDiv');
    $('<button type="button" id="btn_Persons" class="btn btn-primary">Bookings</button>').on('click', event => {
        showBookings();
    }).appendTo('#mainDiv');
    $('<div id="subDiv">').appendTo('body');

    function showPersons() {
        $('#subDiv').empty();
        $('<table class="table" id="tblPersons">').appendTo('#subDiv');
        $('#tblPersons').empty();
        $('<th scope="col">#</th><th scope="col">Vorname</th><th scope="col">Nachname</th><th scope="col">Alter</th></tr></th>').appendTo('#tblPersons');
        $.getJSON('https://localhost:5001/Persons').then(data => {
            data.forEach(x => {
                $('<tr id=row' + x.id + ">")
                    .append('<th scope="row">' + x.id + '</th>')
                    .append('<td">' + x.firstname + '</td>')
                    .append('<td>' + x.lastname + '</td>')
                    .append('<td>' + x.age + '</td>')
                    .appendTo('#tblPersons')
            });
        }
        );
    }
    function showBookings() {
        $('#subDiv').empty();
        $('<button id="back" class="btn btn-primary">Back</button>').on('click', event => {
            lowerKw();
        }).appendTo('#subDiv');
        $('<div id="lbl">').appendTo('#subDiv');
        $(`<label id="#lblCalendarWeek" value=${kw}>`).html(`KW:${kw}`).appendTo('#lbl');

        $('<button id="next" class="btn btn-primary">Next</button>').on('click', event => {
            higherKw();
        }).appendTo('#subDiv');
        displayTable();
    }
    function higherKw() {
        if (kw < 52) {
            kw = kw + 1;
            $("#inputs").empty();
            $("#lbl").empty();
            $("#addEditDiv").empty();
            $(`<label id="#lblCalendarWeek" value=${kw}>`).html(`KW: ${kw}`).appendTo('#lbl');
        }
        displayTable();
    }
    function lowerKw() {
        $("#addEditDiv").empty();
        $("#inputs").empty();
        if (kw > 1) {
            kw = kw - 1;
            $("#lbl").empty();
            $('<label id="#lblCalendarWeek">').html(`KW: ${kw}`).appendTo('#lbl');
        }
        displayTable();
    }
    function displayTable() {
        $('#tblBookings').empty();
        var array = ['Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa', 'So'];
        var array1 = ['10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22'];
        $('<table class="table table-bordered pagin-table" id="tblBookings">').appendTo('#subDiv');
        $('<thead id="head">').appendTo('#tblBookings');
        $('<tr id="rrr">').appendTo('#head');

        $(`<th scope="col" id="hr">`).html("Hour").appendTo('#rrr')
        for (var i = 0; i < array.length; i++) {
            $(`<th scope="col" id=tc${i + 1}>`).html(array[i]).appendTo('#rrr')
        }
        $('<tbody id="bodyt">').appendTo('#tblBookings');
        for (var i = 0; i < array1.length; i++) {
            $(`<tr id=${array1[i]}>`).appendTo('#bodyt');
            $(`#${array1[i]}`).append(`<td  class="col-md-1" scope="row">${array1[i]}:00</td>`)
        }
        for (var i = 0; i < array1.length; i++) {
            for (var j = 0; j < array.length; j++) {
                $(`#${array1[i]}`).append(`<td class="col-md-1">`)
            }
        }

        $('<div id="inputs">').appendTo('#subDiv');
        $('<button class="btn btn-primary" id="addBtn">Hinzufügen</button>').appendTo('#inputs');
        $('<button class="btn btn-primary" id="editBtn">Editieren</button>').appendTo('#inputs')
        $('<input type="text" id="dayOfWeek" class="form-control" placeholder="Wochentag" aria-label="Wochentag" aria-describedby="basic-addon1">').appendTo('#inputs')
        $('<input type="text" id="hour" class="form-control" placeholder="Stunde" aria-label="Stunde" aria-describedby="basic-addon1">').appendTo('#inputs')
        $('<input type="text" id="kw" class="form-control" placeholder="Kalenderwoche" aria-label="Kalenderwoch" aria-describedby="basic-addon1">').appendTo('#inputs')

        $('#addBtn').on('click', event => {

            var calendarWeek = document.getElementById("kw").value;
            var dayOfWeek = document.getElementById("dayOfWeek").value;
            var hour = document.getElementById("hour").value;

            $.ajax({
                url: 'https://localhost:5001/Bookings',
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                data: `{
                        "week": ${parseInt(calendarWeek)},
                        "dayOfWeek": ${parseInt(dayOfWeek)},
                        "hour": ${parseInt(hour)},
                        "personId": ${parseInt(selected)}
                      }`,
            });
            var uhrzeit = $(`#${hour}`);
            $.getJSON('https://localhost:5001/Persons/' + selected).then(data => {
                if (uhrzeit.find(`td:eq(${dayOfWeek})`).html() == '') {
                    uhrzeit.find(`td:eq(${dayOfWeek})`).append(`${data.firstname} ${data.lastname}`);
                }
            })
        });

        var selected;
        $('<select id="personSelect">').appendTo('#inputs')
        $.getJSON('https://localhost:5001/Persons').then(data => {

            data.forEach(x => {
                $(`<option id="${x.id}">`)
                    .html(`${x.firstname} ${x.lastname}`)
                    .val(x.id)
                    .appendTo('#personSelect')

            });

            selected = $("#personSelect option:selected").attr('id');
        });
        $('#personSelect').on('change', event => {
            selected = $("#personSelect option:selected").attr('id');
            console.log(selected);
        });
        console.log(selected);

        $.getJSON('https://localhost:5001/Bookings/calendarWeek/' + kw).then(data => {
            data.forEach(element => {
                var uhrzeit = $(`#${element.hour}`);
                uhrzeit.find(`td:eq(${element.dayOfWeek})`).attr('value', `{"dayOfWeek": ${element.dayOfWeek}, "id": ${element.id}, "hour": ${element.hour}, "week": ${element.week}, "personId": ${element.personId}}`).append(`${element.personDTO.firstname} ${element.personDTO.lastname}`);
            });
        });
        var bookingedit;
        $("td").click(function () {
            bookingedit = JSON.parse($(this).attr('value'));

            document.getElementById("kw").value = bookingedit.week;
            document.getElementById("dayOfWeek").value = bookingedit.dayOfWeek;
            document.getElementById("hour").value = bookingedit.hour;
            $('#personSelect').val(bookingedit.personId);

            console.log(bookingedit);
        });

        $('#editBtn').on('click', event => {

            var calendarWeek = document.getElementById("kw").value;
            var dayOfWeek = document.getElementById("dayOfWeek").value;
            var hour = document.getElementById("hour").value;
            $.ajax({
                url: 'https://localhost:5001/Bookings/' + bookingedit.id,
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                data: `{
                        "week": ${parseInt(calendarWeek)},
                        "dayOfWeek": ${parseInt(dayOfWeek)},
                        "hour": ${parseInt(hour)},
                        "personId": ${parseInt(selected)}
                      }`
            });
            var uhrzeitprev = $(`#${bookingedit.hour}`);
            var uhrzeit = $(`#${hour}`);
            $.getJSON('https://localhost:5001/Persons/' + selected).then(data => {
                uhrzeitprev.find(`td:eq(${bookingedit.dayOfWeek})`).html('');
                uhrzeit.find(`td:eq(${dayOfWeek})`).html('');
                uhrzeit.find(`td:eq(${dayOfWeek})`).attr('value', `{"dayOfWeek": ${dayOfWeek}, "id": ${bookingedit.id}, "hour": ${hour}, "week": ${calendarWeek}, "personId": ${selected}}`).append(`${data.firstname} ${data.lastname}`);
            })
        });
    }
});