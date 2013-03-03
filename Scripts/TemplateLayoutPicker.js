(function ($) {

    $(function() {
        $(".layout-picker-wrapper").on("change", "select", function() {
            var id = $(this).val();
            var link = $(this).parents(".layout-picker-wrapper:first").find("a[data-href]");
            link.attr("href", link.data("href").replace("id", id));

            if (id == "")
                link.hide();
            else
                link.show();
        });
    });
})(jQuery);