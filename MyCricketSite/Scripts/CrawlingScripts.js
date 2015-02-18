$(function () {

    $('body').on('click', '#btnCrawlTeams', function (event) {
        $.ajax({
            cache: false,
            async: true,
            type: "Get",
            url: getSiteUrl() + "/Home/CrawlTeams",
            //    data: $form.serialize() + "&currenturl=" + window.location.toString(),
            success: function (res) {
                var CrawlData = res.CrawlData;
                var teams = [];
                $(CrawlData).find('#ctl00_main_rdgTeams_ctl00 tbody > tr').each(function () {
                    var tdata = [];
                    $('td', this).each(function () {
                        tdata.push($(this).html());
                    });
                    teams.push(tdata);
                });



                $.ajax({
                    cache: false,
                    async: true,
                    type: "POST",
                    url: getSiteUrl() + "/Home/CrawlTeams",
                    data: { teams: JSON.stringify(teams) },
                    success: function (res) {

                    }
                    , error: function (jqXHR, textStatus, errorThrown) {
                        var str;
                    }
                });
            }
            , error: function (jqXHR, textStatus, errorThrown) {
                var str;
            }
        });
    });
});