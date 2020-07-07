/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.enterMode = CKEDITOR.ENTER_BR;
    config.toolbar = 'Full';
    config.filebrowserBrowseUrl = '/Content/themes/ClipOne/plugins/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Content/themes/ClipOne/plugins/ckfinder/ckfinder.html?type=Images';
    config.filebrowserFlashBrowseUrl = '/Content/themes/ClipOne/plugins/ckfinder/ckfinder.html?type=Flash';
    config.filebrowserUploadUrl = '/Content/themes/ClipOne/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Content/themes/ClipOne/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/Content/themes/ClipOne/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    config.filebrowserWindowWidth = '1000';
    config.filebrowserWindowHeight = '700';
    config.language = 'vi';
    config.extraPlugins = 'html5audio,html5video,youtube,widget,widgetselection,lineutils';
};
