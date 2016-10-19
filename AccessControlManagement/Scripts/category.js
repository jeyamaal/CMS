function CategorySaveChanges()
{
    $("#btn-edit-cat").click(function () {
        console.log("Came into button click");

        var category_name = $("#txt-edit-category-name").val();
        var new_category_name = $("#txt-edit-new-category-name").val();

        console.log(category_name+" name of the cat , "+new_category_name + " new cat name");

        $.post("/Categories/Update", {
            oldCategoryName: category_name,
            newCategoryName: new_category_name
        }, function (result) {
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
        var name = $(this).attr('id').split('-')[4];
        console.log(name);

        $.post("/Categories/DeleteCategory",
            { category: name },
            function (result) {
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
    });
}