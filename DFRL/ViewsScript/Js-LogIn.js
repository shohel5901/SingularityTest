jQuery.ajaxSetup({ cache: false });
var getUrl = window.location;
var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";
var token = 'accessToken';
var loginNotification = $("#loginNotification").kendoNotification({
    appendTo: "#errorappendto",
    autoHideAfter: 7000,
    button: true,
    hideOnClick: true,
    stacking: "up",
    animation: {
        close: {
            effects: "collapseVertical"
        }
    }
}).data("kendoNotification");
function logInFunction() {
    if ($("#LogInForm").valid()) {
        var UserEmail = $('#exampleInputEmail').val();
        var UserPassword = $('#exampleInputPassword').val();
        $.ajax({
            url: baseUrl + "api/UserLoginApi",
            type: 'POST',
            datatype: 'json',
            data: {
                UserEmail: UserEmail,
                UserPassword: UserPassword
            },
            beforeSend: function (xhr) {
                var auth = 'Bearer ' + token;
                xhr.setRequestHeader('Authorization', auth);
            },
            success: function (data) {
                $("input[type='exampleInputPassword']").val('');
                localStorage.setItem('userName', data.UserName);
                localStorage.setItem('userId', data.UserId);
                localStorage.setItem('IsCreatePermission', data.IsCreatePermission);
                localStorage.setItem('IsEditPermission', data.IsEditPermission);
                localStorage.setItem('IsViewPermission', data.IsViewPermission);
                localStorage.setItem('IsDeletePermission', data.IsDeletePermission);
                localStorage.setItem('accessToken', data.Token);
                window.location.href = "/home/Index";
                return false;
            },
            error: function (jqXHR) {
                $(".se-pre-con").hide();
                if (jqXHR.status == '400') {
                    loginNotification.show("Username or password is incorrect or inactive", "warning");
                    $("#exampleInputEmail").focus();
                }
                if (jqXHR.status == '401') {
                    loginNotification.show("You are Unauthorized", "warning");
                    $("#exampleInputEmail").focus();
                }
            }
        });
    }
}
$("#LogInForm").validate({
    rules: {
        exampleInputEmail: {
            required: true
        },
        exampleInputPassword: {
            required: true
        }
    },
    messages: {
        exampleInputEmail: {
            required: 'Email is required !',
        },
        exampleInputPassword: {
            required: 'Password is required !',
        }
    },
    errorElement: "div"
});
$("#btnLogIn").unbind().bind('click', function (e) {
    e.preventDefault();
    logInFunction();
});
$("#LogInForm").keyup(function (event) {
    if (event.keyCode === 13) {
        logInFunction();
    }
});