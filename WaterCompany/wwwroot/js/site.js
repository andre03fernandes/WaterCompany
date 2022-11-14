$(document).ready(function () {
    $('#example').DataTable();
});

$(function () {
    setTimeout(function () {
        $('body').removeClass('loading');
    }, 1000);
});
