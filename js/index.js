// html5media enables <video> and <audio> tags in all major browsers
// External File: http://api.html5media.info/1.1.8/html5media.min.js


// Add user agent as an attribute on the <html> tag...
// Inspiration: http://css-tricks.com/ie-10-specific-styles/
var b = document.documentElement;
b.setAttribute('data-useragent', navigator.userAgent);
//b.setAttribute('data-platform', navigator.platform);

var paket = "Paket ".repeat(12);

// HTML5 audio player + playlist controls...
// Inspiration: http://jonhall.info/how_to/create_a_playlist_for_html5_audio
// Mythium Archive: https://archive.org/details/mythium/
jQuery(function($) {
    var supportsAudio = !!document.createElement('audio').canPlayType;
    if (supportsAudio) {
        var
            playing = false,
            mediaPath = '',
            extension = '',
            tracks = [{
                    "track": 1,
                    "name": "jAcid - WWJD",
                    "length": "05:17",
                    "file": "https://storage.googleapis.com/acid-december2012/2022/jAcid-WWJD",
                    "img": "jAcid.jpg"
                }
                /* {
                    "track": 2,
                    "name": "Jerzz - LAB31",
                    "length": "07:39",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/JERZZ-LAB31",
                    "img": "Jerzz-Logo_3000x3000-geel.jpg"
                }, 
 */
            ],
            trackCount = tracks.length,
            index = 32,
            npAction = $('#npAction'),
            npTitle = $('#npTitle'),
            npImage = $('#npImage'),
            audio = $('#audio1').bind('play', function() {
                playing = true;
                npAction.text('playing');
            }).bind('pause', function() {
                playing = false;
                npAction.text('paused');
            }).bind('ended', function() {
                npAction.text('stopped');
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
            btnPrev = $('#btnPrev').click(function() {
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
            btnNext = $('#btnNext').click(function() {
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
            li = $('#plList li').click(function() {
                var id = parseInt($(this).index());
                if (id !== index) {
                    playTrack(id);
                }
            }),
            loadTrack = function(id) {
                $('.plSel').removeClass('plSel');
                $('#plList li:eq(' + id + ')').addClass('plSel');
                npTitle.text(tracks[id].name);
                npImage.text(tracks[id].img);
                index = id;
                audio.src = mediaPath + tracks[id].file + extension;
            },
            playTrack = function(id) {
                loadTrack(id);
                document.body.style.backgroundImage = "url('abstract-art-1321419_960_720.jpg')";
                audio.play();
            };
        extension = audio.canPlayType('audio/mpeg') ? '.mp3' : audio.canPlayType('audio/ogg') ? '.ogg' : '';
        loadTrack(index);
    }
});