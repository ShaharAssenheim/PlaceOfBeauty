
$(document).ready(function () {
function LightenDarkenColor(col, amt) {
    var usePound = false;
    if (col[0] == "#") {
        col = col.slice(1);
        usePound = true;
    }
    var num = parseInt(col, 16);

    var r = (num >> 16) + amt;

    if (r > 255) r = 255;
    else if (r < 0) r = 0;

    var b = ((num >> 8) & 0x00FF) + amt;

    if (b > 255) b = 255;
    else if (b < 0) b = 0;

    var g = (num & 0x0000FF) + amt;

    if (g > 255) g = 255;
    else if (g < 0) g = 0;

    return (usePound ? "#" : "") + (g | (b << 8) | (r << 16)).toString(16);
}

$(".picker").spectrum({
    color: "#F7F3F0",
    chooseText: "Paint my nails!",
    move: function (color) {
        $('.nail, .contents').css({
            fill: color.toHexString()
        });
    },
    change: function (color) {
        $('.nail, .contents').css({
            fill: color.toHexString()
        });
        $('h1 span').css({
            background: color.toHexString()
        });
        $('#bottleSVG title').text('Current colour: ' + color.toHexString());
    }
});


$(".skin-tone").spectrum({
    showPaletteOnly: true,
    showPalette: true,
    color: '',
    palette: [
        ['#dbb595', '#eebb99', 'dda577', '#b29379', '#60452f']
    ],
    change: function (color) {
        $('.st1').css({
            fill: color.toHexString()
        });
        $('.st3').css({
            fill: LightenDarkenColor(color.toHexString(), 20)
        });
        $('.st5').css({
            fill: LightenDarkenColor(color.toHexString(), -10)
        });
        $('p span').css({
            background: color.toHexString()
        });
    }
});
});