$(document).ready(function () {
    $(".preloader").hide();
});

function ValidateLoginUser() {
    $("#loginError").html("");
    var loginDetail = $('#loginform').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Login/ValidateUser',
        type: 'Post',
        datatype: 'Json',
        data: loginDetail
    }).done(function (result) {

        if (result == "") {
            window.location.href = 'Dashboard/Index';
        }
        else {
            $("#loginError").html(result);
            $(".preloader").hide();
            $("#closeError").show();
        }
    }).fail(function (error) {
        $("#loginError").html(error.statusText);
        $(".preloader").hide();
        $("#closeError").show();
        $(".preloader").hide();
    });

    return false;
}

