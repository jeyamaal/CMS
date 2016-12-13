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
                        $("#loading-place").addClass("still-loading");

                        if (index == 0)
                        {
                            $("#nav-place").text("HOME ");

                            $("#header").fadeTo("1000", 1, function () { });
                            $("#load-view").load("/Categories/Index/0", function () {
                                $("#loading-place").removeClass("still-loading");
                                $(".nav-links").css("pointer-events", "");
                                //CategorySaveChanges();;
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
                            $("#nav-place").html("");
                            $("#nav-place").text("Category");
                            //Settings
                            $("#load-view").load("/Categories/_Setting", function () {
                                $("#nav-time").after("<button id='newButton' data-toggle='modal' class='add-new-btn btn btn-default btn-lg'><i class='fa fa-plus-circle fa-2x'></i></button>");
                                CategorySaveChanges();
                                $(".add-new-btn").removeAttr("id");
                                $(".add-new-btn").attr("data-target", "#add-modal-category");
                                $("#header").fadeTo("1000", 1, function () { });
                                $("#load-view").fadeTo("1000", 1, function () {
                                    $("#loading-place").removeClass("still-loading");
                                    $(".nav-links").css("pointer-events", "");
                                });
                            }).error(function (a) {

                            });
                        }
                        else if(index == 3)
                        {
                            // Profile
                            $("#load-view").load("/Home/ProfileView", function () {
                                //CategorySaveChanges();
                                //$(".add-new-btn").removeAttr("id");
                                // $(".add-new-btn").attr("data-target", "#add-modal-category");
                                $("#header").fadeTo("1000", 1, function () { });
                                $("#load-view").fadeTo("1000", 1, function () {
                                    $("#loading-place").removeClass("still-loading");
                                    $(".nav-links").css("pointer-events", "");
                                });
                            }).error(function (a) {

                            });
                        }
                    });

                })(index);
            }
        });
    });


});

