function CategorySaveChanges() {
    $("#btn-add-cat").click(function () {
        console.log("Came into ADD button");

        var new_category_name = $("#txt-add-category-name").val();
        console.log("New Catgeory name: " + new_category_name);

        $.post("/Categories/AddNewCategory", {
            category_name: new_category_name
        }, function (result) {
            $("#load-view").load("/Categories/_Setting", function () {
                CategorySaveChanges();
            });
            console.log(result);
            if (result == "Success") {
                console.log("success");
                $("#add-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                swal("Inserted!", "New Category " + new_category_name + " has been Inserted.", "success");
            }

            else if (result == "The Category is already exists.")
            {
                console.log("The Category is already exists.");
                $("#add-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                swal("Not Inserted!", "The Category is already exists.", "warning");
            }

            else if (result == "The Category " + new_category_name + " is again inserted.") {
                console.log("The Category is already exists.");
                $("#add-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                swal("Inserted!", "The Category " + new_category_name + " is again inserted.", "Success");
            }
            else if (result == "Not Inserted") {
                console.log("Not Inserted");
                $("#add-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                swal("Not Inserted!", "New Category " + new_category_name + " has not been Inserted.", "Error");
            }
            else if (result == "Number of count") {
                console.log("Number of count");
                $("#add-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                swal("Not Inserted!", "New Category " + new_category_name + " has not been Inserted because Database is Empty", "warning");
            }
            else {
                console.log("came to error");
                $("#add-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                swal("Not Inserted!", "New Category " + new_category_name + " has not been Inserted!! Try Again.", "Error");
            }
        }).error(function (e) {

        });
    });


    $(".category-edit").on('click', function () {
        console.log("Came into button click");
        var cid = $(this).attr('id').split('-')[3];
        console.log(cid);
        var category_name = $("#txt-edit-category-name-" + cid).val();
        var new_category_name = $("#txt-edit-new-category-name-" + cid).val();

        console.log(category_name + " name of the cat , " + new_category_name + " new cat name");

        $.post("/Categories/Update", {
            oldCategoryName: category_name,
            newCategoryName: new_category_name
        }, function (result) {
            $("#load-view").load("/Categories/_Setting", function () {
                CategorySaveChanges();
            });
            if (result == "Success") {

                $("#edit-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                alertify.success("Successfully Updated Category");
            }

            else if (result == "Not Success") {

                $("#edit-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                alertify.error("The Category was not updated Successfully");
            }
            else {
                $("#edit-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                alertify.error(result);
            }
        }).error(function (e) {
        });
    });

    $(".category-delete").on('click', function () {
        var cid = $(this).attr('id').split('-')[3];
        console.log(cid);

        $.ajax({
            url: "/Categories/GetCategoryName",
            data: {
                categoryID: cid
            },
            type: "POST",
            dataType: "json",
            success: function (result) {
                console.log(result);
                if (result == "Not Such Value Exists") {
                    $("#load-view").load("/Categories/_Setting/", { partial: true }, function () {
                        CategorySaveChanges();
                    });
                    $("#delete-modal-category-" + cid).hide();
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();
                    alertify.error(result);
                }
                else {
                    console.log("Came to else");
                    $.post("/Categories/DeleteCategory",
                        { category: result },
                        function (result) {
                            $("#load-view").load("/Categories/_Setting/", function () {
                                CategorySaveChanges();
                            });
                            if (result == "Success") {
                                $("#delete-modal-category").hide();
                                $('body').removeClass('modal-open');
                                $('.modal-backdrop').remove();
                                alertify.success("Successfully Category " + name + "Deleted");
                            }
                            else if (result == "Not Success") {
                                $("#delete-modal-category").hide();
                                $('body').removeClass('modal-open');
                                $('.modal-backdrop').remove();
                                alertify.error("Category " + name + "is not Successfuly Deleted");
                            }
                            else {
                                $("#delete-modal-category").hide();
                                $('body').removeClass('modal-open');
                                $('.modal-backdrop').remove();
                                alertify.error(result);
                            }
                        }).error(function (e) {
                            console.log(e);
                        });
                }
            },
            error: function (e) {
            }
        });

    });

    $(".category-edit-advertisement").on('click', function () {
        console.log("Inside Update");
        var aid1 = $(this).attr('id').split('-')[3];
        //var e = document.getElementById("status_type");
        //var type = e.options[e.selectedIndex].text;

        var type = $('#status_type').val();
        console.log(type);
        if (type == "Rejected") {
            $.ajax({
                url: "/Categories/ChangeAdStatus",
                data: {
                    adID: aid1,
                    statusAD: "Reject"
                },
                type: "POST",
                dataType: "json",
                success: function (result) {
                    console.log(result);
                    if (result == "Status is successfully Changed!") {
                        $("#load-view").load("/Categories/_Advertisement/", { partial: true }, function () {
                            CategorySaveChanges();
                        });
                        $("#edit-moda-status-advertisment-" + aid1).hide();
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        swal("Updated!", "Your response have been sent.", "success");
                    }
                    else if (result == "Status is not Changed!") {
                        $("#load-view").load("/Categories/_Advertisement/", { partial: true }, function () {
                            CategorySaveChanges();
                        });
                        $("#edit-moda-status-advertisment-" + aid1).hide();
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        swal("Not Updated!", "Issues while sending the response.", "warning");
                    }
                    else
                    {
                        $("#load-view").load("/Categories/_Advertisement/", { partial: true }, function () {
                            CategorySaveChanges();
                        });
                        $("#edit-moda-status-advertisment-" + aid1).hide();
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        swal("Not Updated!", "Network issues. Please re-try again", "error");
                    }
                },
                error: function (e) {
                }
            });
        }
        else if (type == "Accepted") {
            $.ajax({
                url: "/Categories/ChangeAdStatus",
                data: {
                    adID: aid1,
                    statusAD: "Accept"
                },
                type: "POST",
                dataType: "json",
                success: function (result) {
                    console.log(result);
                    if (result == "Status is successfully Changed!") {
                        $("#load-view").load("/Categories/_Advertisement/", function () {
                            CategorySaveChanges();
                        });
                        $("#edit-moda-status-advertisment-" + aid1).hide();
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        swal("Updated!", "Your response have been sent.", "success");
                    }
                    else if (result == "Status is not Changed!") {
                        $("#load-view").load("/Categories/_Advertisement/", function () {
                            CategorySaveChanges();
                        });
                        $("#edit-moda-status-advertisment-" + aid1).hide();
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        swal("Not Updated!", "Issues while sending the response.", "warning");
                    }
                    else {
                        $("#load-view").load("/Categories/_Advertisement/", function () {
                            CategorySaveChanges();
                        });
                        $("#edit-moda-status-advertisment-" + aid1).hide();
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        swal("Not Updated!", "Network issues. Please re-try again", "error");
                    }
                },
                error: function (e) {
                }
            });
        }
    });

    $(".user-edit-request-advertisement").on('click', function () {
        var reAD = $(this).attr('id').split('-')[4];

        //var date = document.getElementById('txt-edit-posteddate-' + reAD).value;
        //console.log(date);
        //var month = date.split(' ')[0];
        //console.log(month);

        //var monthNo = getMonthName(month);
        //console.log(monthNo);
        //var date = date.split(' ')[1];
        //var year = date.split(' ')[1];
        //console.log(year + " jhsdajs");
        //var expiryDate = year + "-" + monthNo + "-" + date;
        //console.log(expiryDate);

        //var currentdate = new Date();
        //var cuurrentMonth = getMonthNumber(currentdate.getMonth());
        //var currentYear = currentdate.getFullYear();
        //var currentDay = currentdate.getDate();
        //var stringFullDate = currentYear + "-" + cuurrentMonth + "-" + currentDay;
  
        //if (expiryDate < stringFullDate)
        //{
        //    console.log("Expired");
        //}
        //else
        //{
        //    console.log("Not Expired")
        //}

        swal({
            title: "Extension period for Advertisment!",
            text: "Do you want to extend the period for this Advertisment?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Send Request",
            closeOnConfirm: false
        }, 
        function () {
            $.ajax({
                url: "/Categories/RequestExpiraryDate",
                data: {
                    advertisementID: reAD
                },
                type: "POST",
                dataType: "json",
                success: function (result) {
                    console.log(result);
                    if (result == "Requested") {
                        $("#load-view").load("/Categories/_Advertisement/", function () {
                            CategorySaveChanges();
                        });
                        $('.modal-backdrop').remove();
                        swal("Success!", "Request has been sent.", "success");
                    }
                    else if (result == "Request Failed") {
                        $("#load-view").load("/Categories/_Advertisement/", function () {
                            CategorySaveChanges();
                        });
                        $('.modal-backdrop').remove();
                        swal("Failed!", "Request has been not sent.", "Error");
                    }
                    else {
                        $("#load-view").load("/Categories/_Advertisement/", function () {
                            CategorySaveChanges();
                        });
                        $('.modal-backdrop').remove();
                        swal("Not Updated!", "Network issues. Please re-try again", "error");
                    }
                },
                error: function (e) {
                }
            });
        });
    });
}

function getMonthName(month) {
    switch (month) {
        case "Jan": return "01";
            break;
        case "Feb": return "02";
            break;
        case "Mar": return "03";
            break;
        case "Apr": return "04";
            break;
        case "May": return "05";
            break;
        case "Jun": return "06";
            break;
        case "Jul": return "07";
            break;
        case "Aug": return "08";
            break;
        case "Sep": return "09";
            break;
        case "Oct": return "10";
            break;
        case "Nov": return "11";
            break;
        case "Dec": return "12";
            break;
        default: return (month);
    }


}


function getMonthNumber(no) {
    switch (no) {
        case 0: return "01";
            break;
        case 1: return "02";
            break;
        case 2: return "03";
            break;
        case 3: return "04";
            break;
        case 4: return "05";
            break;
        case 5: return "06";
            break;
        case 6: return "07";
            break;
        case 7: return "08";
            break;
        case 8: return "09";
            break;
        case 9: return "10";
            break;
        case 10: return "11";
            break;
        case 11: return "12";
            break;
        default: return (no + 1);
    }


}


function DropDownValueUser() {

    var e = document.getElementById("usernamelist");
    var selectedUser = e.options[e.selectedIndex].value;
    console.log(selectedUser);

    $.ajax({
        url: "/Categories/GetDropDownValueUser",

        data: { user_name: selectedUser },
        type: "GET",
        dataType: "json",
        success: function (data) {
            if (data["Error"] == "Yes") {
                $('#setting-table-created-post').hide();
                $("#table-display").append(
                    $("<div class='col-sm-12' style='font-size:small; text-align: center'></div>").append(
                        $("<label>Error Loading Details..</label>"),
                        $("<hr/>")
                    )
                );                
            } else {
                if (data["Empty"] == "0")
                {
                    drawTableForGroupList(data["details"]);
                }
                else
                {
                    $("#tableStickyNote").hide();
                    $("#sticky-note-display").append(
                        $("<div class='col-sm-12' style='font-size:small; text-align: center'></div>").append(
                            $("<label>No Trips</label>"),
                            $("<hr/>")
                        )
                    );
                }
            }
        },
        error: function (e) {
        }
    });

    //$.post("/Categories/GetDropDownValueUser", {
    //    user_name: selectedUser
    //}, function (result) {
    //    console.log(result);

    //    if (result != null) {
    //        drawTable(result, "");
    //    }
    //    else if (result == "No post has been written by " + selectedUser) {
    //        drawTable(null, "No post written to show");
    //    }
    //    else {
    //        drawTable(null, "No post has been written to show");
    //    }

    //    $("#load-view").load("/Categories/_Post/", function () {
    //        CategorySaveChanges();  
    //    });
        
    //}).error(function (e) {
    //    console.log("error" + e)
    //});  
}

function drawTable(data) {

    $('#setting-table-created-post thead tr').show();

    if (data != null)
    {
        for (var i = 0; i < data.length; i++) {
            drawRow(data[i]);
        }
    }
    else
    {
        $('#setting-table-created-post thead tr').hide();
        $("<label>"+message+"</label>").show();
    }
}

function drawRow(rowData) {

    $('#setting-table-created-post thead tr').show();
    var row = $("<tr />");
    //this will append tr element to table.
    $("#setting-table-created-post").append(row);
    row.append($("<td>" + rowData.categoryName + "</td>"));
    row.append($("<td>" + rowData.date + "</td>"));
    row.append($("<td>" + rowData.title + "</td>"));
    row.append($("<td><button id='delete-btn-category-@item.category_id' type='button' class='btn btn-success' data-toggle='modal' data-target='#delete-modal-category-@item.category_id'><i class='fa fa-eye' aria-hidden='true'></i></button>" +
                     "<button id='delete-btn-category-@item.category_id' type='button' class='btn btn-success' data-toggle='modal' data-target='#delete-modal-category-@item.category_id'><i class='fa fa-eye' aria-hidden='true'></i></button>" +
                "</td>"));
}