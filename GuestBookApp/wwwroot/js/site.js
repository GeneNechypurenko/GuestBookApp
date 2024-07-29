$(document).ready(function () {

    $('#loginLink').on('click', function (event) {
        event.preventDefault();
        $('#loginModal').modal('show');
    });

    $('#registerLink').on('click', function (event) {
        event.preventDefault();
        $('#registerModal').modal('show');
    });

    $('#feedbackLink').on('click', function (event) {
        event.preventDefault();
        $('#feedbackModal').modal('show');
    });

    $('.close').on('click', function () {
        $(this).closest('.modal').modal('hide');
    });

    $('#guestBookForm').off('submit').on('submit', function (event) {
        event.preventDefault();
        console.log('GuestBookForm submitted');

        $.ajax({
            type: 'POST',
            url: $(this).attr('action'),
            data: $(this).serialize(),
            success: function () {
                $('#feedbackModal').modal('hide');
                $.get('/Home/PartialGuestBookEntries', function (data) {
                    $('#entries').html(data);
                });
            },
            error: function () {
                alert('Error submitting message. Please try again.');
            }
        });
    });

    $('#loginForm').submit(function (event) {
        event.preventDefault();
        var form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.data('url'),
            data: form.serialize(),
            success: function () {
                window.location.href = '/Home/Index';
            },
            error: function (response) {
                var errors = response.responseJSON;
                form.find('.text-danger').text('');
                for (var key in errors) {
                    var errorContainer = form.find('[data-valmsg-for="' + key + '"]');
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
            url: form.data('url'),
            data: form.serialize(),
            success: function () {
                window.location.href = '/Home/Index';
            },
            error: function (response) {
                var errors = response.responseJSON;
                form.find('.text-danger').text('');
                for (var key in errors) {
                    var errorContainer = form.find('[data-valmsg-for="' + key + '"]');
                    if (errorContainer.length > 0) {
                        errorContainer.text(errors[key][0]);
                    }
                }
            }
        });
    });

    $('#logoutLink').on('click', function (event) {
        event.preventDefault();
        $.ajax({
            type: 'POST',
            url: '/Account/Logout',
            success: function () {
                window.location.href = '/';
            },
            error: function () {
                alert('Error during logout. Please try again.');
            }
        });
    });
});
