jQuery.ajaxSetup({ cache: false });
var token = localStorage.getItem('accessToken');
var userId = localStorage.getItem('userId');
var pathArray = window.location.pathname;
var getUrl = window.location;
var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";
var toggle = false;
$('#sidebarToggleTop').click(function () {
    toggle = !toggle;
    if (toggle) {
        //$('#accordionSidebar').fadeOut(1000);
        $('#accordionSidebar').hide();
    }
    else {
        // $('#accordionSidebar').fadeIn(1000);
        $('#accordionSidebar').show();
    }
});

function fnLogout() {
    swal({
        title: 'Logout !',
        text: "Are you sure want to logout!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, logout!'
    }).then((result) => {
        if (result.value == true) {
            localStorage.removeItem('accessToken');
            localStorage.removeItem('userName');
            localStorage.removeItem('userId');
            localStorage.clear();
            localStorage.clear();
            token == null;
            window.location.href = "/Home/login";
        }
    })
}

if (token == null) {
    window.location.href = "/Home/Login";
}
$.ajax({
    url: baseUrl + "api/PermittedUserMenu?userId=" + userId + "&pathArray=" + pathArray,
    type: "GET",
    dataType: "json",
    beforeSend: function (xhr) {
        var auth1 = 'Bearer ' + token;
        xhr.setRequestHeader('Authorization', auth1);
    },
    success: function (result) {
        if (result == 202) {
            return true;
        }
    },
    error: function (jqXHR) {
        if (jqXHR.status == 400) {
            var base_url = window.location.origin;
            window.location.href = "/Home/index";
            return false;
        }
    }
});


if (userId == null) {
    window.location.href = "/Home/Login";
}
else {
    $.ajax({
        url: baseUrl + "api/PermittedUserMenu?userId=" + userId,
        type: "GET",
        dataType: "json",
        beforeSend: function (xhr) {
            var auth = 'Bearer ' + token;
            xhr.setRequestHeader('Authorization', auth);
        },
        success: function (result) {
            var navString = "";
            var id = 0;
            $.each(result, function (key, value) {
                navString = navString + '<li class="nav-item">'
                      + '<a class="nav-link collapsed" href="#" data-toggle="collapse" data-target="#collapseBootstrap' + id + '" aria-expanded="true" aria-controls="collapseBootstrap">'
                      + '<i class="' + value.parent_icon_class + '"></i>'
                      + ' <span>' + value.parent_menu_text + '</span></a>';
                if (value.tb_user_sub_menu.length > 0) {
                    navString = navString + '<div id="collapseBootstrap' + id + '" class="collapse" aria-labelledby="headingBootstrap" data-parent="#accordionSidebar">'
                           + '<div class="bg-white py-2 collapse-inner rounded"><h6 class="collapse-header">' + value.parent_menu_text + '</h6>';
                    $.each(value.tb_user_sub_menu, function (key1, value1) {
                        navString = navString + '<a class="collapse-item" href="' + value1.page_url + '">' + value1.sub_menu_text + '</a>';
                    });
                    navString = navString + ' </div></div></li>';
                    id++;
                }
                else {
                    navString = navString + '</li>';
                }
            });
            $("#menuContent").append(navString);
        }
    });
}