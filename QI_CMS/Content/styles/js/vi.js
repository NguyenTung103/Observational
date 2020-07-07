//! moment.js locale configuration
//! locale : vietnamese (vi)
//! author : Bang Nguyen : https://github.com/bangnk

(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(require('../moment')) :
        typeof define === 'function' && define.amd ? define(['moment'], factory) :
            factory(global.moment)
}(this, function (moment) {
    'use strict';


    var vi = moment.defineLocale('vi', {
        months: 'Tháng 1_Tháng 2_Tháng 3_Tháng 4_Tháng 5_Tháng 6_Tháng 7_Tháng 8_Tháng 9_Tháng 10_Tháng 11_Tháng 12'.split('_'),
        monthsShort: 'Th01_Th02_Th03_Th04_Th05_Th06_Th07_Th08_Th09_Th10_Th11_Th12'.split('_'),
        weekdays: 'chủ nhật_thứ hai_thứ ba_thứ tư_thứ năm_thứ sáu_thứ bảy'.split('_'),
        weekdaysShort: 'CN_T2_T3_T4_T5_T6_T7'.split('_'),
        weekdaysMin: 'CN_T2_T3_T4_T5_T6_T7'.split('_'),
        //longDateFormat: {
        //    LT: 'HH:mm',
        //    LTS: 'HH:mm:ss',
        //    L: 'DD/MM/YYYY',
        //    LL: 'D MMMM [năm] YYYY',
        //    LLL: 'D MMMM [năm] YYYY HH:mm',
        //    LLLL: 'dddd, D MMMM [năm] YYYY HH:mm',
        //    l: 'DD/M/YYYY',
        //    ll: 'D MMM YYYY',
        //    lll: 'D MMM YYYY HH:mm',
        //    llll: 'ddd, D MMM YYYY HH:mm'
        //},
        //calendar: {
        //    sameDay: '[Hôm nay lúc] LT',
        //    nextDay: '[Ngày mai lúc] LT',
        //    nextWeek: 'dddd [Tuần tới lúc] LT',
        //    lastDay: '[Hôm qua lúc] LT',
        //    lastWeek: 'dddd [Tuần trước lúc] LT',
        //    sameElse: 'L'
        //},
        //relativeTime: {
        //    future: '%s tá»›i',
        //    past: '%s trÆ°á»›c',
        //    s: 'vÃ i giÃ¢y',
        //    m: 'má»™t phÃºt',
        //    mm: '%d phÃºt',
        //    h: 'má»™t giá»',
        //    hh: '%d giá»',
        //    d: 'má»™t ngÃ y',
        //    dd: '%d ngÃ y',
        //    M: 'má»™t Tháng',
        //    MM: '%d Tháng',
        //    y: 'má»™t nÄƒm',
        //    yy: '%d nÄƒm'
        //},
        ordinalParse: /\d{1,2}/,
        ordinal: function (number) {
            return number;
        },
        week: {
            dow: 1, // Monday is the first day of the week.
            doy: 4  // The week that contains Jan 4th is the first week of the year.
        }
    });

    return vi;

}));