//Validation for OnRecoverpassword 
function OnRecoverPassword() {

    var mymail = $("#emailID").val();
    //Inhere I use  "url:window.location.pathname + 'Home/RecoverPassword/'", for accessing javascript in another file
    // other wise calling dirctly:-  "url: '@Url.Action("RecoverPassword", "Home")',"
    // --Jeyamaal
    $.ajax({
        type: "POST",
        url:window.location.pathname + 'Home/RecoverPassword/',
        data: { "email": mymail },
        success: function (data) {

            if (data == "WrongEmail") {
                sweetAlert("Invalid Email", "Input the correct Email Address!", "error");
            }
            else {
                swal("Email has sent!", "Please Check email for passoword!", "success");
            }

        }

    });

}


// Validation Login
function OnLogin(content) {

    $(document).ready(function () {
        $('#loading-image').show();
        
    });

    var username = $("#username").val();
    var password = $("#password").val();

    console.log(username);
    console.log(password);

    //console.log(mymail);
    //inhere I use  "url:window.location.pathname + 'Home/Login/'", for accessing javascript in another file
    // other wise calling dirctly:-  "url: '@Url.Action("RecoverPassword", "Home")',"
    // --Jeyamaal

    $.ajax({
        type: "POST",
        url: window.location.pathname + 'Home/Login/',
        data: { "un": username, "pwd": password },


        success: function (data) {
           if (data == "WrongCredits") {

               sweetAlert("Invalid UserName or Password", "Input the correct credentials!", "error");

               //hide the login loading image
               $(document).ready(function () {
                    $('#loading-image').hide();

                });
            }

            else {
                window.location.replace("home/AfterLogin");
            }


        }

    });

}
