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