//#region Apache License
/**
 * Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
//#endregion

(function () {
    'use strict';

    angular.module('strixDirectives').directive('strixTinyMce', ['$rootScope', '$timeout', function ($rootScope, $timeout) {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                var editorElement = element[0];
                var settings = getSettings();
                tinyMCE.init(getTinyMceConfig(editorElement, settings));
                scope.$on('entityLoaded', updateEditor);

                function getSettings() {
                    var theElement = $(editorElement);

                    var settings = {
                        url: theElement.attr('rteurl'),
                        advanced: theElement.attr('useAdvancedOptions'),
                        comments: theElement.attr('useCommentsOptions'),
                        fileUploads: theElement.attr('useFileUploads') == 'true' ? "addmedia," : "",
                        codeEditing: theElement.attr('codeEditing') == 'true' ? true : false
                    };

                    settings.plugins = settings.fileUploads + "pagebreak,style,layer,table,advimage,advlink,iespell,inlinepopups,insertdatetime,preview,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist";
                    settings.buttonRowOne =
                        settings.advanced == 'true' ?
                        "fullscreen,|,preview,|,undo,redo,|,cut,copy,paste,pastetext,pasteword,|,search,replace,|,bold,italic,underline,strikethrough,|,sub,sup,|,formatselect,|,bullist,numlist,|,outdent,indent,blockquote" :
                        settings.comments == 'true' ?
                        "bold,italic,underline,strikethrough,|,bullist,numlist,|,link,unlink" :
                        "fullscreen,|,undo,redo,|,cut,copy,paste,pastetext,pasteword,|,search,replace,|,bold,italic,underline,strikethrough,bullist,numlist";
                    settings.buttonRowTwo =
                        settings.advanced == 'true' ?
                        "link,unlink,anchor,image,addmedia,|,tablecontrols,|,hr,removeformat,visualaid,|,charmap,|,print,|,nonbreaking,restoredraft" :
                        settings.comments == 'true' ?
                        "" :
                        "link,unlink,|,hr,removeformat,visualaid,|,charmap,|";

                    if (settings.codeEditing) {
                        settings.buttonRowTwo = "code,|," + settings.buttonRowTwo;
                    }

                    return settings;
                }

                function getTinyMceConfig(element, settings) {
                    return {
                        content_css: strixIT.config.resourceRootUrl + 'Areas/Cms/Styles/tinymce.css',
                        script_url: strixIT.config.resourceRootUrl + 'Areas/Cms/TinyMCE/tiny_mce.js',

                        // General options
                        mode: "exact",
                        elements: element.id,
                        theme: "advanced",
                        plugins: settings.plugins,

                        // Theme options
                        theme_advanced_buttons1: settings.buttonRowOne,
                        theme_advanced_buttons2: settings.buttonRowTwo,
                        theme_advanced_buttons3: "",

                        addmedia_action: settings.url,
                        request_verification_token: strixIT.getRequestVerificationToken(),

                        theme_advanced_toolbar_location: "top",
                        theme_advanced_toolbar_align: "left",
                        theme_advanced_statusbar_location: "bottom",
                        theme_advanced_resizing: true,

                        // Drop lists for link/image/media/template dialogs
                        template_external_list_url: "lists/template_list.js",
                        external_link_list_url: "lists/link_list.js",
                        external_image_list_url: "lists/image_list.js",
                        media_external_list_url: "lists/media_list.js",

                        // Style formats
                        style_formats: [
                            { title: 'Bold text', inline: 'b' },
                            { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
                            { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
                            { title: 'Example 1', inline: 'span', classes: 'example1' },
                            { title: 'Example 2', inline: 'span', classes: 'example2' },
                            { title: 'Table styles' },
                            { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
                        ],

                        // Change from local directive scope -> "parent" scope
                        // Update Textarea and Trigger change event
                        // you can also use handle_event_callback which fires more often
                        onchange_callback: editorChanged
                    }
                }

                function updateEditor(event, entity) {
                    $timeout(function () {
                        var editor = tinymce.get(editorElement.id);

                        if (editor) {
                            var value = entity[ngModel.$name];

                            if (value) {
                                editor.setContent(entity[ngModel.$name]);
                            }

                            tinyMCE.execCommand('mceRemoveEditor', false, editorElement.id);
                            tinyMCE.execCommand('mceAddEditor', false, editorElement.id);
                        }
                    }, 50);
                }

                function editorChanged() {
                    var editor = this;

                    $timeout(function () {
                        if (editor.isDirty()) {
                            editor.save();

                            // tinymce inserts the value back to the textarea element, so we get the val from element (works only for textareas)
                            var value = editor.getContent();
                            ngModel.$setViewValue(value);
                            return true;
                        }
                    }, 0);
                }
            }
        }
    }]);
})()