// html5media enables <video> and <audio> tags in all major browsers
// External File: http://api.html5media.info/1.1.8/html5media.min.js


// Add user agent as an attribute on the <html> tag...
// Inspiration: http://css-tricks.com/ie-10-specific-styles/
var b = document.documentElement;
b.setAttribute('data-useragent', navigator.userAgent);
b.setAttribute('data-platform', navigator.platform);


// HTML5 audio player + playlist controls...
// Inspiration: http://jonhall.info/how_to/create_a_playlist_for_html5_audio
// Mythium Archive: https://archive.org/details/mythium/
jQuery(function ($) {
    var supportsAudio = !!document.createElement('audio').canPlayType;
    if (supportsAudio) {
        var index = 0,
            playing = false,
            mediaPath = '',
            extension = '',
            tracks = [{
                "track": 1,
                "name": "The Electones: Stepnine",
                "length": "08:17",
                "file": "https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_01_-_The_Electones_-_Stepnine"
            }, {
                "track": 2,
                "name": "Doktorns Elektriska Arm: Letting Down Your Heir MKII",
                "length": "07:14",
                "file": "https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_02_-_Doktorns_elektriska_arm_-_Letting_down_your_heir_MKll"
            }, {
                "track": 3,
                "name": "Jätten: Polar Beats",
                "length": "05:02",
                "file": "https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_03_-_Jätten_-_Polar_beats"
            }, {
                "track": 4,
                "name": "PO-TA-TIS",
                "length": "08:32",
                "file": "4_PO_TA_TIS"
            }, {
                "track": 5,
                "name": "Skala på, skala på",
                "length": "05:05",
                "file": "5_Skala_pa_skala_pa"
            }, {
                "track": 6,
                "name": "Förra året",
                "length": "02:49",
                "file": "6_Forra_aret"
            }],
            trackCount = tracks.length,
            npAction = $('#npAction'),
            npTitle = $('#npTitle'),
            audio = $('#audio1').bind('play', function () {
                playing = true;
                npAction.text('Now Playing...');
            }).bind('pause', function () {
                playing = false;
                npAction.text('Paused...');
            }).bind('ended', function () {
                npAction.text('Paused...');
                if ((index + 1) < trackCount) {
                    index++;
                    loadTrack(index);
                    audio.play();
                } else {
                    audio.pause();
                    index = 0;
                    loadTrack(index);
                }
            }).get(0),
            btnPrev = $('#btnPrev').click(function () {
                if ((index - 1) > -1) {
                    index--;
                    loadTrack(index);
                    if (playing) {
                        audio.play();
                    }
                } else {
                    audio.pause();
                    index = 0;
                    loadTrack(index);
                }
            }),
            btnNext = $('#btnNext').click(function () {
                if ((index + 1) < trackCount) {
                    index++;
                    loadTrack(index);
                    if (playing) {
                        audio.play();
                    }
                } else {
                    audio.pause();
                    index = 0;
                    loadTrack(index);
                }
            }),
            li = $('#plList li').click(function () {
                var id = parseInt($(this).index());
                if (id !== index) {
                    playTrack(id);
                }
            }),
            loadTrack = function (id) {
                $('.plSel').removeClass('plSel');
                $('#plList li:eq(' + id + ')').addClass('plSel');
                npTitle.text(tracks[id].name);
                index = id;
                audio.src = mediaPath + tracks[id].file + extension;
            },
            playTrack = function (id) {
                loadTrack(id);
                audio.play();
            };
        extension = audio.canPlayType('audio/mpeg') ? '.mp3' : audio.canPlayType('audio/ogg') ? '.ogg' : '';
        loadTrack(index);
    }
});