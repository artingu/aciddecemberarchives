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
            },
                {
                'track': 3,
                'name': "Jätten: Polar Beats",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_03_-_Jätten_-_Polar_beats'
            }, {
                'track': 4,
                'name': "Perceptionsdepartement: Sensorium",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_04_-_Perceptionsdepartementet_-_Sensorium'
            }, {
                'track': 5,
                'name': "Flumberman: Acid Chimney",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_05_-_Flumberman_-_Acid_Chimney'
            }, {
                'track': 6,
                'name': "Heathen State: They Have No Ideals",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_06_-_Heathen_State_-_They_Have_No_Ideals'
            }, {
                'track': 7,
                'name': "Sunflowr: Sleep Paralysis",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_07_-_Sunflowr_-_Sleep_Paralysis'
            }, {
                'track': 8,
                'name': "Betty Ford: Menorah",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_08_-_Betty_Ford_-_Menorah'
            }, {
                'track': 9,
                'name': "Sir John Keason: Knorr",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_09_-_Sir_Jon_Keason_-_Knorr'
            }, {
                'track': 10,
                'name': "Batterimännen: Rätt Sjyst2",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_10_-_Batterimaennen_-_Raett_Sjyst2'
            }, {
                'track': 11,
                'name': "Kotten: Pin Tweaks",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_11_-_Kotten_-_Pin_Tweaks'
            }, {
                'track': 12,
                'name': "Flumberman: Acorn",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_12_-_Flumberman_-_Acorn'
            }, {
                'track': 13,
                'name': "Aelvis Praestlig: White and Bleu Christmuss",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_13_-_aelvis_praestlig_-_white_and_bleu_chrismuss'
            }, {
                'track': 14,
                'name': "Modvol: Milkpecker",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_14_-_Modvol_-_Milkpecker'
            }, {
                'track': 15,
                'name': "Handysound: Shake the Stew",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_15_-_Handysound_-_Shake_The_Stew'
            }, {
                'track': 16,
                'name': "SH13: Korgtastic",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_16_-_SH13_-_Korgtastic'
            }, {
                'track': 17,
                'name': "Simio Sakrecoer: 7 Inch Spike",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_17_-_Simio_Sakrecoer_-_7_Inch_Spike'
            }, {
                'track': 18,
                'name': "Kotten o Salkin: Mer Acid",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_18_-_Kotten_o_Salkin_-_Mer_acid'
            }, {
                'track': 19,
                'name': "Nordbeck: Acid 1101",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_19_-_Nordbeck_-_Acid_1101'
            }, {
                'track': 20,
                'name': "Sgt Dickweed: Von Oben",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_20_-_Sgt_Dickweed_-_Von_Oben'
            }, {
                'track': 21,
                'name': "J Hansson: Slag 2",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_21_-_J_Hansson_-_Slag_2'
            }, {
                'track': 22,
                'name': "Morglurg: MDF Funk",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_22_-_Morglurg_-_MDF_Funk'
            }, {
                'track': 23,
                'name': "Gemini: Intent",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_23_-_Gemini-_Intent'
            }, {
                'track': 24,
                'name': "Viktor Pilkington: Nippon 303",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_24_-_Viktor_Pilkington_-_Nippon_303'
            }, {
                'track': 25,
                'name': "Goto80: ACID BURGER",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_25_-_Goto80_-_ACID_BURGER'
            }, {
                'track': 26,
                'name': "DJ Svante Arrhenius: Woof",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_26_-_DJ_Svante_Arrhenius_-_Woof'
            }, {
                'track': 27,
                'name': "Morusque: Mangeoire disjonctive",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_27_-_Morusque_-_Mangeoire_disjonctive'
            }, {
                'track': 28,
                'name': "Bitcrusher: ASCIID 01",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_28_-_Bitcrusher_-_ASCIID_01'
            }, {
                'track': 29,
                'name': "JMPH: Under a Foreign Sky",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_29_-_jmph_-_Under_a_Foreign_Sky'
            }, {
                'track': 30,
                'name': "Hardhat: Impact Driver",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_30_-_Hardhat_-_Impact_Driver'
            }, {
                'track': 31,
                'name': "Kallback: 090909drumatix",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_31_-_kallback_-_090909drumatix'
            }, {
                'track': 32,
                'name': "FALCON CRUST: Codex Syra",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_32_-_FALCON_CRUST_-_Codex_Syra'
            }, {
                'track': 33,
                'name': "Arayaz: Arayaz Anthem",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_33_-_Arayaz_-_Arayaz_Anthem'
            }, {
                'track': 34,
                'name': "Spionen: Farsta Slott",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_34_-_Spionen_-_Farsta_Slott'
            }, {
                'track': 35,
                'name': "Skruv: Skare",
                    'length': '05:02',
                'file': 'https://storage.googleapis.com/acid-december2012/2013/Acid_December_2013_-_35_-_Skruv_-_Skare'
            },


            ],
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