$(document).ready(function () {
    $('#guestBookForm').submit(function (event) {
        event.preventDefault();
        var form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            success: function (response) {
                $('#entries').html(response);
                form[0].reset();
            },
            error: function (response) {
                var errors = response.responseJSON;
                form.find('.text-danger').text('');
                for (var key in errors) {
                    var errorContainer = form.find('[asp-validation-for="' + key + '"]');
                    if (errorContainer.length > 0) {
                        errorContainer.text(errors[key][0]);
                    }
                }
            }
        });
    });

    $('#loginForm').submit(function (event) {
        event.preventDefault();
        var form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            success: function () {
                location.reload();
            },
            error: function (response) {
                var errors = response.responseJSON;
                form.find('.text-danger').text('');
                for (var key in errors) {
                    var errorContainer = form.find('[asp-validation-for="' + key + '"]');
                    if (errorContainer.length > 0) {
                        errorContainer.text(errors[key][0]);
                    }
                }
            }
        });
    });

    $('#registerForm').submit(function (event) {
        event.preventDefault();
        var form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            success: function () {
                location.reload();
            },
            error: function (response) {
                var errors = response.responseJSON;
                form.find('.text-danger').text('');
                for (var key in errors) {
                    var errorContainer = form.find('[asp-validation-for="' + key + '"]');
                    if (errorContainer.length > 0) {
                        errorContainer.text(errors[key][0]);
                    }
                }
            }
        });
    });

    $('#logoutBtn').click(function () {
        $.post('/Account/Logout', function () {
            location.reload();
        });
    });

    $.get('/Home/GetMessages', function (response) {
        $('#entries').html(response);
    });
});