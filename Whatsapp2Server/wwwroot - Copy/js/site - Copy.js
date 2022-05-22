$(function () {
    $('form').submit(e => {
        e.preventDefault();

        const q = $('#search').val();

        $('tbody').load('/Users/Search2?query='+q);
    })
});
