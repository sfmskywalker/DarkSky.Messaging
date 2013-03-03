(function ($) {

    var layoutDictionary = {};
    var currentLayoutContent = null;

    var getLayoutContent = function (layoutPicker) {

        if (!currentLayoutContent) {
            var layoutId = parseInt(layoutPicker.val());
            currentLayoutContent = layoutDictionary[layoutId];

            if (!currentLayoutContent && !isNaN(layoutId)) {
                var url = layoutPicker.data("load-layout-url") + "/" + layoutId;
                var promise = $.ajax({
                    url: url
                });

                promise.done(function(html) {
                    layoutDictionary[layoutId] = html;
                    currentLayoutContent = html;
                });

                return promise;
            }
        }

        return $.Deferred().resolve(currentLayoutContent);
    };

    var updatePreview = function (editor, previewFrame, layoutPicker) {
        var frame = previewFrame[0];
        var preview = frame.contentDocument || frame.contentWindow.document;
        var content = editor.getValue();
        var layoutContentPromise = getLayoutContent(layoutPicker);

        layoutContentPromise.done(function (layoutContent) {
            if (layoutContent != null && layoutContent != "") {
                content = layoutContent.replace("@RenderBody()", content);
            }
            preview.open();
            preview.write(content);
            preview.close();
        });
        
    };

    var initializeEditors = function() {
        $("fieldset.message-template-editor").each(function () {
            var textArea = $(this).find("textarea.template-editor")[0];
            var layoutPicker = $(this).find(".layout-selector-wrapper select");
            var previewPane = $(this).find("iframe");
            var editor = CodeMirror.fromTextArea(textArea, {
                lineNumbers: true,
                mode: "application/x-ejs",
                indentUnit: 4,
                indentWithTabs: true,
                enterMode: "keep",
                tabMode: "shift",
                theme: "vibrant-ink",
                autoCloseTags: true
            });
            
            var delay = 10;
            var delayTimerCallback = function () { updatePreview(editor, previewPane, layoutPicker); };
            var delayTimer = setTimeout(delayTimerCallback, delay);

            editor.on("change", function () {
                clearTimeout(delayTimer);
                delayTimer = setTimeout(delayTimerCallback, delay);
            });

            layoutPicker.on("change", function () {
                currentLayoutContent = null;
                updatePreview(editor, previewPane, layoutPicker);
            });
        });
    };

    $(function() {
        initializeEditors();
    });
})(jQuery);