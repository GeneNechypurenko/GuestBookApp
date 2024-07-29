$(document).ready(function () {
    // Обработчик клика для открытия модального окна входа
    $('#loginLink').on('click', function (event) {
        event.preventDefault();
        $('#loginModal').modal('show');
    });

    // Обработчик клика для открытия модального окна регистрации
    $('#registerLink').on('click', function (event) {
        event.preventDefault();
        $('#registerModal').modal('show');
    });

    // Существующий код для отправки форм
    $('#loginForm').submit(function (event) {
        event.preventDefault(); // Prevent the default form submission
        var form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            success: function (response) {
                window.location.href = '/Home/Index'; // Redirect to home page
            },
            error: function (response) {
                // Handle validation errors
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
        event.preventDefault(); // Prevent the default form submission
        var form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            success: function (response) {
                window.location.href = '/Account/Login'; // Redirect to login page
            },
            error: function (response) {
                // Handle validation errors
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
});
