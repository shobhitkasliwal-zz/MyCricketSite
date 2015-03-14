var AllTournaments = [];
function PopulateTournament() {
    $.ajax({
        cache: false, async: true, type: "Get", url: getSiteUrl() + "/Home/GetAllTournamentDisplay",
        success: function (res) {

            if (res != null && res.toString().length > 0) {
                var tournaments = JSON.parse(res.Tournaments);

                var ActiveTournaments = [];
                $.each(tournaments, function (k, v) {
                    AllTournaments.push(v);
                    if (v.Status === 'Active') {
                        ActiveTournaments.push(v);
                    }
                });
                if (ActiveTournaments.length == 1) {
                    SaveCurrentTournament(ActiveTournaments[0].EntityId);
                }
                else {
                    ShowTournamentPopup();
                }

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //sendEmail(jqXHR, "BindWalletCreationCancelEvent");
        }
    });
}
$('body').on('click', '#divTournamentNamePlaceholder', function () { PopulateTournament(); ShowTournamentPopup(); });


var ShowTournamentPopup = function () {
    $('#divSelectTouramentPopupError').html('');
    var htmlvalue = "<div style='text-align:center;'><br>Please select the tournament from the list below:<br><br><select id='ddlSelectPopupTournament'><option value=''> -- Select -- </option>";
    for (var i = 0 ; i < AllTournaments.length ; i++) {
        htmlvalue = htmlvalue + "<option value='" + AllTournaments[i].EntityId + "'> " + AllTournaments[i].Name + " </option>";
    }
    htmlvalue = htmlvalue + '<select><br><br><input type="Checkbox" id="chkSaveDefaulttournament">&nbsp;&nbsp;Remember<br><br><input type="button" id="btnSelectTournamentOk" value="OK" class="btn btn-default" /><br><br>';
    OpenPopup("<div class='white-popup-block' id='divSelectTouramentPopup'  style='max-width:450px;'><div class='PopupTitle'>Select Tournament</div><div class='PopupContent'>" + htmlvalue + "</div></div>");
    $('.mfp-close').remove();
    $.magnificPopup.instance.close = function () { };

    $('body').on('click', '#btnSelectTournamentOk', function (event) {
        if ($('#ddlSelectPopupTournament').val().length <= 0) {
            $(event.target).parent().append('<div id="divSelectTouramentPopupError" style="color:red;"> Please select the tournament </div><br>');
        }
        else {
            SaveCurrentTournament($('#ddlSelectPopupTournament').val(), $('#chkSaveDefaulttournament').is(':checked'));
            $.magnificPopup.proto.close.call(this);
            $.magnificPopup.instance = null;
        }

    });


};

var SaveCurrentTournament = function (entityid, saveondevice) {

    $.ajax({
        cache: false, async: true, type: "Post", url: getSiteUrl() + "/Home/SelectTournament",
        data: { EntityId: entityid, saveondevice: saveondevice },
        success: function (res) {

            if (res != null && res.toString().length > 0) {
                if (res.Result === "SUCCESS")
                {
                    $('#spanCurrentTournament').html(res.TournamentName);
                }

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //sendEmail(jqXHR, "BindWalletCreationCancelEvent");
        }
    });

};


var PopulateHomeGames = function () {
    
    $.ajax({
        cache: false, async: true, type: "Get", url: getSiteUrl() + "/Home/GetAllGamesForCurrentTournament",
        success: function (res) {

            if (res != null && res.toString().length > 0) {
                var games = JSON.parse(res.Dates);
                var html = "";
                var TournamentStartDate = eval(res.TournamentStartDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
                $.each(games, function (k, v) {
                    //var date = new Date(v.GameDate);
                    var date = eval(v.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
                    //var dt = new Date(v.GameDate);
                    html += "<a class='slide'>" + (weeks_between(date, TournamentStartDate) + 1) +"<br>" + date.customFormat("#DDD# #MMM# #DD# #YYYY#") + "</a>"
                });
              $('#divGameSlider').html(html);
                $('.slider1').bxSlider({
                    slideWidth: 200,
                    minSlides: 1,
                    maxSlides:6,
                    slideMargin: 10,
                    moveSlides: 1,
                    pager: false,
                    pagerType: 'short'
                });
               
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //sendEmail(jqXHR, "BindWalletCreationCancelEvent");
        }
    });

}


function weeks_between(date1, date2) {
    // The number of milliseconds in one week
    var ONE_WEEK = 1000 * 60 * 60 * 24 * 7;
    // Convert both dates to milliseconds
    var date1_ms = date1.getTime();
    var date2_ms = date2.getTime();
    // Calculate the difference in milliseconds
    var difference_ms = Math.abs(date1_ms - date2_ms);
    // Convert back to weeks and return hole weeks
    return Math.floor(difference_ms / ONE_WEEK);
}
