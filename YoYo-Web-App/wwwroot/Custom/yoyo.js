var startArray;
var endArray;
var speed;
var speedLevel;
var shuttleNo;
var distance;
var levelTime;
var trackTime;

var interval;
var ArraySelector = 0;

var playerCount;
let warnedPlayer=0;
let stopedPlayer=0;

$(document).ready(function () {

    //Clear_Inerval()

    $.ajax({
        type: "POST",
        url: '/Home/TimerListAjax',
        success: function (result) {
            startArray = result[0].startTimer;
            endArray = result[0].endTimer;
            speed = result[0].speed;
            speedLevel = result[0].speedLevel;
            shuttleNo = result[0].shuttleNo;
            levelTime = result[0].levelTime;
            distance = result[0].distance;
            playerCount = result[0].playerCount;

            if (getCookie("trackTime") &&
                getCookie("ArraySelector") &&
                getCookie("allsecond") &&
                getCookie("time") &&
                getCookie("variation") &&
                getCookie("i")) {                
                StartTimer();
            }

            if (getCookie("warnedPlayer")) {
                $("#btnStart").hide();
                warnedPlayer = parseInt(getCookie("warnedPlayer"));                
                for (var w = 0; w <= warnedPlayer; w++) {
                    if (result[0].existingPlayer[w].warned !== "") {
                        const Id = parseInt(result[0].existingPlayer[w].id, 10);
                        $('#warn_' + Id).text('Warned');
                        $('#warn_' + Id).prop('disabled', true);
                        $('#stop_' + Id).prop('disabled', false);
                    }
                }
            }
            if (getCookie("stopedPlayer")) {
                $("#btnStart").hide();
                stopedPlayer = parseInt(getCookie("stopedPlayer"), 10);                
                for (var s = 0; s <= stopedPlayer; s++) {
                    if (result[0].existingPlayer[s].stoped !== "") {
                        const Id = parseInt(result[0].existingPlayer[s].id, 10);
                        $('#stop_' + Id).text('Stoped');                        
                        $('#stop_' + Id).prop('disabled', true);

                        $("#shuttle_" + Id).text(result[0].existingPlayer[s].shuttleNo);
                        $("#level_" + Id).text(result[0].existingPlayer[s].speedLevel);
                        $('#ddl_' + Id).show();
                    }
                }
            }
            
        },
        error: function (result) {
            alert('error');
        }
    });
});

function StartTimer() {
    $("#btnStart").hide();
    $(".colWarn").show();
    $(".colStop").show();

    var StartStringtimer = startArray[ArraySelector];
    var starttime = StartStringtimer.split(':');
    var startminutes = parseInt(starttime[0], 10);
    var startseconds = parseInt(starttime[1], 10);

    if (getCookie("trackTime") &&
        getCookie("ArraySelector")) {
        ArraySelector = parseInt(getCookie("ArraySelector"), 10);
        StartStringtimer = getCookie("trackTime");
        starttime = StartStringtimer.split(':');
        startminutes = parseInt(starttime[0], 10);
        startseconds = parseInt(starttime[1], 10);
    }
    setCookie("ArraySelector", ArraySelector);

    var EndStringtimer = endArray[ArraySelector];
    var endtime = EndStringtimer.split(':');
    var endminutes = parseInt(endtime[0], 10);
    var endseconds = parseInt(endtime[1], 10);

    var allsecond = endseconds - startseconds + (endminutes * 60) - (endminutes * 60);
    var time = allsecond;
    var variation = 440 / allsecond;
    var i = 0;

    if (getCookie("allsecond") &&
        getCookie("time") &&
        getCookie("variation") &&
        getCookie("i")) {

        allsecond = parseInt(getCookie("allsecond"), 10);
        time = parseInt(getCookie("time"), 10);
        variation = parseInt(getCookie("variation"), 10);
        i = parseInt(getCookie("i"), 10);

        $('.circle_animation').css('stroke-dashoffset', 440 - (i * (variation)));
    }
    else {
        $('.circle_animation').css('stroke-dashoffset', 440 - (1 * (variation)));
    }
    setCookie("allsecond", allsecond);
    setCookie("time", time);
    setCookie("i", i);
    setCookie("variation", variation);

    interval = setInterval(function () {
        $('#speedLevel').text('Level: ' + speedLevel[ArraySelector]);
        $('#shuttleTimer').text('Shuttle: ' + shuttleNo[ArraySelector]);
        $('#kmh').text(speed[ArraySelector] + ' km/h');

        $('#shuttle').text(levelTime[ArraySelector] + ' s.');
        $('#time').text(startminutes + ':' + startseconds + ' m.');
        if (ArraySelector > 0) { $('#distance').text(distance[ArraySelector - 1] + ' m.'); }
        trackTime = startminutes + ':' + startseconds;
        setCookie("trackTime", trackTime);

        startseconds++;
        if (startseconds === 60) {
            startminutes++;
            startseconds = 0;
        }

        if (i === time) {
            //clearInterval(interval);
            $('#distance').text(distance[ArraySelector] + ' m.');

            ArraySelector++;
            setCookie("ArraySelector", ArraySelector);

            $('#shuttle').text(levelTime[ArraySelector + 1] + ' s.');
            $('#time').text(startminutes + ':' + startseconds + ' m.');

            $('#speedLevel').text('Level: ' + speedLevel[ArraySelector]);
            $('#shuttleTimer').text('Shuttle: ' + shuttleNo[ArraySelector]);
            $('#kmh').text(speed[ArraySelector] + ' km/h');

            trackTime = startminutes + ':' + startseconds;
            setCookie("trackTime", trackTime);

            StartStringtimer = startArray[ArraySelector];
            starttime = StartStringtimer.split(':');
            startminutes = parseInt(starttime[0], 10);
            startseconds = parseInt(starttime[1], 10);

            EndStringtimer = endArray[ArraySelector];
            endtime = EndStringtimer.split(':');
            endminutes = parseInt(endtime[0], 10);
            endseconds = parseInt(endtime[1], 10);

            allsecond = (endseconds - startseconds) + (endminutes * 60) - (startminutes * 60);
            setCookie("allsecond", allsecond);

            time = allsecond;
            setCookie("time", time);

            variation = 440 / allsecond;
            setCookie("variation", variation);

            i = 0;
            setCookie("i", i);
        }

        $('.circle_animation').css('stroke-dashoffset', 440 - ((i + 1) * (variation)));

        i++;
        setCookie("i", i);
    }, 1000);
}

function warnPlayer(Id) {
    $.ajax({
        type: "POST",
        url: "/Home/WarnPlayer/",
        data: {
            id: Id,
            warnTime: trackTime
        },
        success: function (result) {
            if (result.success) {
                $('#warn_' + Id).text('Warned');
                $('#warn_' + Id).prop('disabled', true);
                $('#stop_' + Id).prop('disabled', false);

                if (getCookie("warnedPlayer")) {
                    setCookie("warnedPlayer", warnedPlayer++);
                }
                else {
                    warnedPlayer++
                    setCookie("warnedPlayer", warnedPlayer);
                }
            }
            else {
                console.log('error: ', result.error);
            }
        },
        error: function (result) {
            console.log('error');
        }
    });
}

function stopPlayer(Id) {
    var arrayIndex;
    if (ArraySelector > 0) {
        arrayIndex = ArraySelector - 1;
    }
    else {
        arrayIndex = ArraySelector;
    }

    $.ajax({
        type: "POST",
        url: "/Home/StopPlayer/",
        data: {
            id: Id,
            stopTime: trackTime,
            shuttle: shuttleNo[arrayIndex],
            level: speedLevel[ArraySelector]
        },
        success: function (result) {
            if (result.success) {
                $('#stop_' + Id).prop('disabled', true);
                $('#stop_' + Id).text('Stoped');
                $('#ddl_' + Id).show();

                $("#shuttle_" + Id).text(result.success.shuttleNo);
                $("#level_" + Id).text(result.success.speedLevel);
                if (getCookie("stopedPlayer")) {
                    setCookie("stopedPlayer", stopedPlayer++);
                }
                else {
                    setCookie("stopedPlayer", stopedPlayer++);
                }

                if (stopedPlayer === playerCount) {
                    Clear_Inerval();
                }
            }
            else {
                console.log('error: ', result.error);
            }
        },
        error: function (result) {
            console.log('error');
        }
    });
}

function Clear_Inerval() {
    clearCookie("trackTime");
    clearCookie("ArraySelector");
    clearCookie("allsecond");
    clearCookie("time");
    clearCookie("variation");
    clearCookie("i");
    clearCookie("stopedPlayer");
    clearCookie("warnedPlayer");
    clearInterval(interval);

    $('#distance').text('0 m.');
    $('#shuttle').text('0 s.');
    $('#time').text('0 m.');

    $('#speedLevel').text('Level: 0');
    $('#shuttleTimer').text('Shuttle: 0');
    $('#kmh').text('0 km/h');
}

//cookie functions
function setCookie(cname, cvalue) {
    var d = new Date();
    d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function clearCookie(name) {
    document.cookie = name + '=;Path=/;Expires=Thu, 01 Jan 1970 00:00:01 GMT;'
}
//cookie functions