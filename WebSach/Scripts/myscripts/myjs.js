function show_bottom_menu(target) {
    if ($(target).attr('class') == "fa fa-list") {
        $(target).attr('class', 'fa fa-window-close');
        $("#header_left_menu").attr('class', 'menu_mobile');
    } else {
        $(target).attr('class', 'fa fa-list');
        $("#header_left_menu").attr('class', '');
    }
}