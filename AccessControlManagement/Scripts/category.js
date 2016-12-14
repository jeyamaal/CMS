function CategorySaveChanges()
{
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
                alertify.success("Successfully Inserted Category");
            }
            else if (result == "Not Inserted")
            {
                console.log("Not Inserted");
                $("#add-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                alertify.error("Not Successfully Inserted Category");
            }
            else if (result == "Number of count") {
                console.log("Number of count");
                $("#add-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                alertify.error("Database is empty");
            }
            else
            {
                console.log("came to error");
                $("#add-modal-category").hide();
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                alertify.error("Error");
            }
        }).error(function (e) {

        });
    });


    $(".category-edit").on('click', function () {
        console.log("Came into button click");
        var cid = $(this).attr('id').split('-')[3];
        console.log(cid);
        var category_name = $("#txt-edit-category-name-"+cid).val();
        var new_category_name = $("#txt-edit-new-category-name-"+cid).val();

        console.log(category_name+" name of the cat , "+new_category_name + " new cat name");

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
                    $("#delete-modal-category-"+cid).hide();
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


}