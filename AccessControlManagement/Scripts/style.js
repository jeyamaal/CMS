$(document).ready(function () {

    $(".nav-links").each(function (index) {

        $(this).click(function () {

            if (index != 4) {
                $("#home-link").css('color', '');
                $("#home-link").css('background-color', '');

                $(".nav-clicked").addClass("effect-1");
                $(".nav-clicked").addClass("effect-2");
                $(".nav-clicked").removeClass("nav-clicked");

                $(this).removeClass("effect-1");
                $(this).removeClass("effect-2");
                $(this).addClass("nav-clicked");

                $(".nav-links").css("pointer-events", "none");

                (function (index) {
                    $("#header").fadeTo("1000", 0, function () { });
                    $("#load-view").fadeTo("1000", 0, function () {

                        if (index == 0) {


                        } else if (index == 1) {
                                
                        } else if (index == 2) {
                            //Settings
                            $("#load-view").load("/Categories/Index", function () {
                                CategorySaveChanges();
                                //$(".add-new-btn").removeAttr("id");
                                //$(".add-new-btn").attr("data-target", "#add-new-usedby-model");

                                //$("#header").fadeTo("1000", 1, function () { });
                                //$("#load-view").fadeTo("1000", 1, function () {
                                //    $("#loading-place").removeClass("still-loading");
                                //    $(".nav-links").css("pointer-events", "");
                                //});
                            }).error(function (a) {

                            });
                        } else {

                        }
                    });

                })(index);
            }
        });
    });


});

