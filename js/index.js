// html5media enables <video> and <audio> tags in all major browsers
// External File: http://api.html5media.info/1.1.8/html5media.min.js


// Add user agent as an attribute on the <html> tag...
// Inspiration: http://css-tricks.com/ie-10-specific-styles/
var b = document.documentElement;
b.setAttribute('data-useragent', navigator.userAgent);
b.setAttribute('data-platform', navigator.platform);

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
                    "name": "Kotten - Acid my ass",
                    "length": "05:17",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Kotten_-_Acid_my_ass",
                    "img": "OIP.jpg"
                }, {
                    "track": 2,
                    "name": "Jerzz - LAB31",
                    "length": "07:39",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/JERZZ-LAB31",
                    "img": "Jerzz-Logo_3000x3000-geel.jpg"
                }, {
                    "track": 3,
                    "name": "Jazzdoktorn - Entartete Kunst",
                    "length": "08:42",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Jazzdoktorn-Entartete-kunst-11",
                    "img": "pink.jpg"
                }, {
                    "track": 4,
                    "name": "Shadow Traxx - Get Sorted",
                    "length": "05:50",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Shadow_Traxx-Get_Sorted!",
                    "img": "SHADOW_TRAXX_SOUND_CLOUD_PERFIL.png"
                }, {
                    "track": 5,
                    "name": "MOTHER JOONAS - Bumbulum",
                    "length": "04:13",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/MOTHER_JOONAS-Bumbulum",
                    "img": "MOTHER JOONAS Bumbulum Cover.jpeg"
                }, {
                    "track": 6,
                    "name": "Sebsongs - Acid Radio Santa",
                    "length": "05:44",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/sebsongs-acid_radio_santa",
                    "img": "OIP.jpg"
                }, {
                    "track": 7,
                    "name": "Chimär - Kolbotn Acid Session I",
                    "length": "04:42",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Kolbotn_Acid_Sessions_I",
                    "img": "OIP.jpg"
                }, {
                    "track": 8,
                    "name": "Shike - Knäcktuggaren",
                    "length": "04:23",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Knacktuggaren",
                    "img": "OIP.jpg"
                }, {
                    "track": 9,
                    "name": "Coop Xtra - Bra produkter till ett lågt pris",
                    "length": "04:31",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Coop_Xtra-Bra_produkter_till_l%C3%A5gt_pris",
                    "img": "OIP.jpg"
                }, {
                    "track": 10,
                    "name": "Lies & Sets - Bra Sill",
                    "length": "02:58",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/2021-Acid_December_2021-07-Bra_Sill-Lies_%26_Sets",
                    "img": "redherring.jpg"
                }, {
                    "track": 11,
                    "name": "TSR - Vaegen svengde men ikke renen",
                    "length": "03:03",
                    "file": " https://storage.googleapis.com/acid-december2012/2021/tsr_-_vaegen_svengde_men_ikke_renen",
                    "img": "OIP.jpg"
                }, {
                    "track": 12,
                    "name": "nSIx - AC#D",
                    "length": "05:28",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/AC%23D",
                    "img": "OIP.jpg"
                }, {
                    "track": 13,
                    "name": "Moelodin - Toppad",
                    "length": "05:28",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/moelodin%20toppad",
                    "img": "13.jpg"
                }, {
                    "track": 14,
                    "name": "Robotbror - WA02",
                    "length": "02:40",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Robotbror_WA-02",
                    "img": "image0.jpeg"
                },
                {
                    "track": 15,
                    "name": "Erko - Draperi acid",
                    "length": "12:40",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/draperi-acid",
                    "img": "erko.jpg"
                }, {
                    "track": 16,
                    "name": "jAcid - Glöggsyra",
                    "length": "07:08",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/jAcid-Gloggsyra",
                    "img": "jacidcolorbeatport.jpg"
                }, {
                    "track": 17,
                    "name": "Kuvös X - Bronsålderskollapsen (Acid Mix)",
                    "length": "05:36",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Kuvos%20X_Bronsalderskollapsen(Acid%20mix)",
                    "img": "image1.jpeg"
                }, {
                    "track": 18,
                    "name": "Chimär - Kolbotn Acid Session II",
                    "length": "05:21",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Kolbotn_Acid_Sessions_II",
                    "img": "D700A683.jpg"
                }, {
                    "track": 19,
                    "name": "Kotten - Apparatkontroll",
                    "length": "03:48",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Kotten%20-%20Apparatkontroll",
                    "img": "07DE1747.jpg"
                }, {
                    "track": 20,
                    "name": "JGB - Syrliga Julhälningar",
                    "length": "04:39",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/jgb-syrliga_julhalsningar",
                    "img": "637219AD.jpg"
                },{
                    "track": 21,
                    "name": "Klabbarparna - Stolpapaera i G dur",
                    "length": "02.15",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Klabbarparna%20-%20Stolpapaera%20i%20G%20dur",
                    "img": "04A247B7.JPG"
                },{
                    "track": 22,
                    "name": "Hjorten - Korgsyra",
                    "length": "02.13",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/korgsyra",
                    "img": "04A247B7.JPG"
                },{
                    "track": 23,
                    "name": "Linusolinus - Tomtemors rengryta",
                    "length": "05.12",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/linusolinus%20-%20Tomtemors%20rengryta",
                    "img": "abstract-art-1321419_960_720.jpg"
                },{
                    "track": 24,
                    "name": "Erko - Plink Plonk Acid", 
                    "length": "04.05",
                    "file" : "https://storage.googleapis.com/acid-december2012/2021/plinkplonk-acid"
                }, {
                    "track": 25,
                    "name" : "Reefer Sutherland - Ringmod för livet", 
                    "length": "3.33",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Reefer%20Sutherland%20-%20Ringmod%20f%C3%B6r%20livet"
                }, {
                    "track": 26,
                    "name" : "nSix - Improvacadition", 
                    "length": "3.28",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/Improvacadition"
                }, {
                    "track": 27,
                    "name" : "nSix - Improvacadition", 
                    "length": "3.28",
                }, {
                    "track": 28,
                    "name" : "Patric B - Acid Monotonie", 
                    "length": "4.53",
                    "file": "https://storage.googleapis.com/acid-december2012/2021/patrick_b-acid_monotonie"
                    }
                

            ],
            trackCount = tracks.length,
            index = 27,
            npAction = $('#npAction'),
            npTitle = $('#npTitle'),
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
                index = id;
                audio.src = mediaPath + tracks[id].file + extension;
            },
            playTrack = function(id) {
                loadTrack(id);
                audio.play();
            };
        extension = audio.canPlayType('audio/mpeg') ? '.mp3' : audio.canPlayType('audio/ogg') ? '.ogg' : '';
        loadTrack(index);
    }
});