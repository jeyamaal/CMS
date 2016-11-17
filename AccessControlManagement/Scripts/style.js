$(document).ready(function () {

    $(".nav-links").each(function (index) {

        $(this).click(function () {

            if (index != 3) {
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

                        if (index == 0)
                        {
                            $("#load-view").load("/Categories/Index", function () {
                                //CategorySaveChanges();
                                //$(".add-new-btn").removeAttr("id");
                                //$(".add-new-btn").attr("data-target", "#add-modal-category");

                                //$("#header").fadeTo("1000", 1, function () { });
                                //$("#load-view").fadeTo("1000", 1, function () {
                                //    $(".nav-links").css("pointer-events", "");
                                //});
                            }).error(function (a) {

                            });
                        }
                        else if (index == 1)
                        {
                            //$("#load-view").load("/Categories/_Post", {partial:true}, function () {
                                
                            //}).error(function (a) {

                            //});
                        }
                        else if (index == 2)
                        {
                            //Settings
                            $("#load-view").load("/Categories/_Setting", {partial: true }, function () {
                                CategorySaveChanges();
                                $(".add-new-btn").removeAttr("id");
                                $(".add-new-btn").attr("data-target", "#add-modal-category");

                                $("#header").fadeTo("1000", 1, function () { });
                                $("#load-view").fadeTo("1000", 1, function () {
                                    $(".nav-links").css("pointer-events", "");
                                });
                            }).error(function (a) {

                            });
                        }
                        else
                        {

                        }
                    });

                })(index);
            }
        });
    });


});

