(function ($) {

    SocialFeedsObject = function (el, options) {
        this.init(el, options);
    };

    $.extend(SocialFeedsObject.prototype, {
        
        init: function (el, options) {

            this.defaults = {
                feeds: {
                    facebook: {
                        id: '',
                        intro: 'Posted',
                        out: 'intro,title,picture,text,user,share',
                        text: 'content',
                        comments: 3,
                        image_width: 4, //3 = 600 4 = 480 5 = 320 6 = 180,
                        url: '',
                        feed: 'feed', // feed, posts
                        icon: 'facebook.png',
                        priority: 0
                    },
                    twitter: {
                        id: '',
                        intro: 'Tweeted',
                        search: 'Tweeted',
                        out: 'intro,thumb,text,share',
                        retweets: false,
                        replies: false,
                        images: '', // large w: 786 h: 346, thumb w: 150 h: 150, medium w: 600 h: 264, small w: 340 h 150
                        url: 'twitter.php',
                        icon: 'twitter.png',
                        priority: 0
                    },
                    youtube: {
                        id: '',
                        intro: 'Uploaded',
                        search: 'Search',
                        out: 'intro,thumb,title,text,user,share',
                        text: 0,
                        api_key: 'AIzaSyB1zptnRspzltRVLGQJMBCH2yYujYp7ytI',
                        thumb: 'medium', //default 120 x 90, medium 320 x 180, high 480 x 360, standard 640 x 480
                        icon: 'youtube.png',
                        priority: 0
                    },
                    flickr: {
                        id: '',
                        intro: 'Uploaded',
                        out: 'intro,thumb,title,text,share',
                        lang: 'en-us',
                        icon: 'flickr.png',
                        priority: 0
                    },
                    pinterest: {
                        id: '',
                        intro: 'Pinned',
                        out: 'intro,thumb,text,user,share',
                        icon: 'pinterest.png',
                        priority: 0
                    },
                    instagram: {
                        id: '',
                        intro: 'Posted',
                        search: 'Search',
                        out: 'intro,thumb,text,user,share,meta',
                        accessToken: '',
                        redirectUrl: '',
                        clientId: '',
                        thumb: 'low_resolution',
                        comments: 3,
                        likes: 8,
                        icon: 'instagram.png',
                        priority: 0
                    }
                },
                remove: '',
                twitterId: '',
                days: 5,
                limit: 10,
                max: 'days',
                external: true,
                speed: 600,
                height: 550,
                wall: false,
                centre: false,
                order: 'date',
                filter: true,
                controls: true,
                rotate: {
                    direction: 'up',
                    delay: 8000
                },
                transition: '0.8s',
                cache: true,
                container: 'feeds',
                cstream: 'stream',
                content: 'feeds-content',
                iconPath: 'images/feeds-dark/',
                imagePath: 'images/feeds-dark/',
                debug: false,
                filterText: 'filter',
                allButton: 'left'
            };

            this.o = {}, this.timer_on = 0, this.id = 'feeds-' + $(el).index(), this.timerId = '', this.o = $.extend(true, this.defaults, options), opt = this.o, $load = $('<div class="feeds-loading">creating stream ...</div>');

            $(el).addClass(this.o.container).append('<div class="' + this.o.content + '"><ul class="' + this.o.cstream + '"></ul></div>');

            var $c = $('.' + this.o.content, el), $a = $('.' + this.o.cstream, el), $l = $('li', $a);

            if (opt.height > 0 && opt.wall == false) {
                $c.css({ height: opt.height + 'px' });
            }

            if (this.o.filter == true || this.o.controls == true) {
                var x = '<div class="feeds-toolbar">';
                if (this.o.filter == true) {
                    var fclass = this.o.center == true ? 'option-set filter feed-center' : 'option-set filter';
                    x += '<ul id="feeds-filter" class="' + fclass + '">';
                    x += this.o.wall == true ? '<li><a class="filter-text"><span>' + opt.filterText + '</span></a></li>' : '';
                    if (opt.allButton === 'left') {
                        x += this.o.wall == true ? '<li class="all-filter"><a href="#filter" data-group="feed-filter" data-filter="*" class="selected link-all iso-active"><span>All</span></a></li>' : '';
                    }
                    var $f = $('.filter', el);

                    var sortedFeeds = [];

                    $.each(opt.feeds, function (k, v) {
                        v.key = k;
                        if (v.id != '') {
                            sortedFeeds.push(v);
                        }
                    });

                    sortedFeeds.sort(function (a, b) {
                        return (b.priority) < (a.priority) ? 1 : -1;
                    });

                    $.each(sortedFeeds, function (k, v) {
                        x += v.id != '' ? '<li class="active f-' + v.key + '"><a href="#filter" rel="' + v.key + '" data-group="feed-filter" data-filter=".feeds-' + v.key + '"><img src="' + v.icon + '" alt="" /></a></li>' : '';
                    });

                    if (opt.allButton === 'right') {
                        x += this.o.wall == true ? '<li class="all-filter"><a href="#filter" data-group="feed-filter" data-filter="*" class="selected link-all iso-active"><span>All</span></a></li>' : '';
                    }
                    x += '</ul>';
                }
                
                x += '</div>';
                if (opt.wall == false) {
                    $(el).append(x);
                } else {
                    $(el).before(x);
                }
            }

            if (this.o.wall == true) {
                $('.feeds-toolbar').append($load);
                var w = $("#feeds-filter.feed-center").width() / 2;
                $("#feeds-filter.feed-center").css({ 'margin-left': -w + "px" }).fadeIn();
                this.createwall($a);
            } else {
                $c.append($load);
            }

            this.createstream(el, $a, 0, opt.days);

            $load.remove();
        },

        createstream: function (obj, s, f1, f2) {
            $.each(opt.feeds, function (k, v) {
                if (opt.feeds[k].id != '') {
                    var txt = [];
                    $.each(opt.feeds[k].intro.split(','), function (i, v) {
                        v = $.trim(v);
                        txt.push(v);
                    });
                    $.each(opt.feeds[k].id.split(','), function (i, v) {
                        v = $.trim(v);
                        if (opt.feeds[k].feed && v.split('#').length < 2) {
                            if (k == 'youtube' && v.split('/').length > 1) {
                                getFeed(k, v, opt.iconPath, opt.feeds[k], obj, opt, f1, f2, 'posted', '', i);
                            } else {
                                $.each(opt.feeds[k].feed.split(','), function (i, feed) {
                                    getFeed(k, v, opt.iconPath, opt.feeds[k], obj, opt, f1, f2, txt[i], feed, i);
                                });
                            }
                        } else {
                            intro = v.split('#').length < 2 ? opt.feeds[k].intro : opt.feeds[k].search;
                            getFeed(k, v, opt.iconPath, opt.feeds[k], obj, opt, f1, f2, intro, '', i);
                        }
                    });
                }
            });
        },

        createwall: function (obj) {
            obj.isotope({
                itemSelector: 'li.feeds-li',
                transitionDuration: opt.transition,
                getSortData: {
                    postDate: function (itemElem) {
                        return parseInt($(itemElem).attr('rel'), 10);
                    }
                },
                sortBy: 'postDate',
                masonry: {
                    isFitWidth: opt.center
                }
            });
        },

        
        pauseTimer: function () {
            clearTimeout(this.timerId);
            this.timer_on = 0;
            $('.controls .pause').removeClass('pause').addClass('play');
        }
    });

    $.fn.SocialNetworkFeedsStream = function (options, callback) {
        var d = {};
        this.each(function () {
            var s = $(this);
            d = s.data("socialtabs");
            if (!d) {
                d = new SocialFeedsObject(this, options, callback);
                s.data("socialtabs", d);
            }
        });
        return d;
    };

    function getFeed(type, id, path, o, obj, opt, f1, f2, intro, feed, fn) {

        var stream = $('.stream', obj), list = [], d = '', px = 300, c = [], data, href, url, n = opt.limit, txt = [], src;
        frl = 'https://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=' + n + '&callback=?&q=';
        frlapi = 'https://api.rss2json.com/v1/api.json?api_key=kczawsvt8tfadg2gyhirgmrleem6yqr4l3qo9xx8&order_by=&count=' + n + '&rss_url=';

        switch (type) {

            case 'facebook':
                var cp = id.split('/');
                var curl = o.url.replace(/\&#038;/gi, "&");
                href = 'https://www.facebook.com/' + id.split('|')[0];
                url = url = cp.length > 1 ? 'https://graph.facebook.com/' + cp[1] + '/photos?fields=id,link,from,name,picture,images,comments&limit=' + n : curl + '?id=' + id + '&limit=' + n + '&feed=' + o.feed + '&type=' + type;
                break;

            case 'twitter':
                var curl = o.url.replace(/\&#038;/gi, "&");
                var cp = id.split('/'), cq = id.split('|')[0].split('#'), cu = o.url.split('?'), replies = o.replies == true ? '&exclude_replies=false' : '&exclude_replies=true';
                var param = '&include_entities=true&include_rts=' + o.retweets + replies;
                url1 = cu.length > 1 ? curl + '&' : curl + '?';
                url = cp.length > 1 ? url1 + 'url=list&list_id=' + cp[1] + '&per_page=' + n + param : url1 + 'url=timeline&screen_name=' + id + '&count=' + n + param;
                if (cq.length > 1) {
                    id = id.replace('#', '');
                    var rts = o.retweets == false ? '+exclude:retweets' : '';
                    url = url1 + 'id=' + id + '&limit=' + n + '&feed=' + '' + '&type=' + type;
                }
                break;

          

            case 'youtube':
                n = n > 50 ? 50 : n;
                var cp = id.split('/'), cq = decodeURIComponent(id).split('#'), cc = id.split('!');
                if (cq.length > 1) {
                    url = 'https://www.googleapis.com/youtube/v3/search?part=snippet&key=' + o.api_key + '&pageToken=&order=date&maxResults=' + n + '&q=' + cq[1];
                    href = 'https://www.youtube.com/results?search_query=' + cq[1];
                } else {
                    if (cc.length > 1) {
                        id = cc[1];
                        id = 'UU' + cc[1].substring(2);
                        href = 'https://www.youtube.com/channel/UC' + id.substring(2);
                    } else {
                        id = cp.length > 1 ? cp[1] : id;
                        if (id.substr(0, 2) != 'UU') {
                            if (id.substr(0, 2) != 'UC') {
                                // is playlist ID
                                href = 'https://www.youtube.com/playlist?list=' + id;
                            } else {
                                // is channel ID
                                href = 'https://www.youtube.com/channel/' + id;
                                id = 'UU' + id.substring(2);
                            }
                        } else {
                            // is list ID
                            href = 'https://www.youtube.com/channel/UC' + id.substring(2);
                        }
                    }
                    url = 'https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId=' + id + '&key=' + o.api_key + '&pageToken=&maxResults=' + n;
                }
                break;

            case 'flickr':
                var cq = id.split('/'), fd = cq.length > 1 ? 'groups_pool' : 'photos_public';
                id = cq.length > 1 ? cq[1] : id;
                href = 'https://www.flickr.com/photos/' + id;
                url = 'https://api.flickr.com/services/feeds/' + fd + '.gne?id=' + id + '&lang=' + o.lang + '&format=json&jsoncallback=?';
                break;

          
            case 'pinterest':
                var cp = id.split('/');
                url = 'https://www.pinterest.com/' + id + '/';
                url += cp.length > 1 ? 'rss' : 'feed.rss';
                href = 'http://www.pinterest.com/' + id;
                url = frlapi + encodeURIComponent(url);
                break;

            case 'instagram':
                href = '#';
                url = 'https://api.instagram.com/v1';
                var cp = id.substr(0, 1), cq = id.split(cp), url1 = cq[1], qs = '', ts = 0;
                switch (cp) {
                    case '?':
                        var p = cq[1].split('/');
                        qs = '&lat=' + p[0] + '&lng=' + p[1] + '&distance=' + p[2];
                        url += '/media/search';
                        break;
                    case '#':
                        url += '/tags/' + url1 + '/media/recent';
                        ts = 1;
                        break;
                    case '!':
                        url += '/users/' + url1 + '/media/recent';
                        break;
                    case '@':
                        url += '/locations/' + url1 + '/media/recent';
                        break;
                }
                if (o.accessToken == '' && ts == 0) {
                    if (location.hash) {
                        o.accessToken = location.hash.split('=')[1];
                    } else {
                        location.href = "https://instagram.com/oauth/authorize/?client_id=" + o.clientId + "&redirect_uri=" + o.redirectUrl + "&response_type=token";
                    }
                }
                url += '?access_token=' + o.accessToken + '&client_id=' + o.clientId + '&count=' + n + qs;
                break;
        }
        var dataType = type == 'twitter' || type == 'facebook' ? 'json' : 'jsonp';
        jQuery.ajax({
            url: url,
            data: data,
            cache: opt.cache,
            dataType: dataType,
            success: function (a) {
                var error = '';
                switch (type) {
                    case 'facebook':
                        if (cp.length > 1) {
                            a = a.data;
                        }
                        break;
                   
                    case 'flickr':
                        a = a.items;
                        break;
                    case 'instagram':
                        a = a.data;
                        break;
                    case 'twitter':
                        error = a.errors ? a.errors : '';
                        break;
                    case 'youtube':
                        a = a.items;
                        break;
                   
                    default:
                        if (a.status == "ok") {
                            a = a.items;
                        } else {
                            error = a.responseDetails;
                        }
                        break;
                }
                if (error == '') {
                    $.each(a, function (i, item) {
                        if (i < n) {
                            var html = [], q = item.link, u = '<a href="' + href + '">' + id + '</a>', w = '', x = '<a href="' + q + '">' + item.title + '</a>', y = '', z = '', zz = '', m = '', d = item.publishedDate, sq = q, st = item.title, s = '', dateloc, thumbnail, picture;
                            switch (type) {
                                case 'facebook':
                                   
                                        x = '', y = '', z = '', u = '', q = '';
                                        if (item.Thumb !== null && item.Thumb !== '') {
                                            thumbnail = item.Thumb;
                                        }
                                        if (thumbnail != null && thumbnail !== '') {
                                            y = '<a href="' + item.Link.Url + '"><img src="' + thumbnail.Src + '" alt="' + thumbnail.Alt + '" /></a>';
                                        }
                                        if (item.Title != null && item.Title !== '') {
                                            u = item.Title;
                                        }
                                        if (item.Link != null && item.Link !== '') {
                                            q = item.Link.Url;
                                        }
                                        if (item.Content !== null && item.Content !== '') {
                                            z = linkify(item.Content);
                                        }
                                        if (item.FullPicture != null && item.FullPicture !== '') {
                                            picture = item.FullPicture;
                                        }
                                        if (item.DateLocation != null && item.DateLocation !== '') {
                                            dateloc = item.DateLocation;
                                            d = item.Date;
                                        }
                                        if (item.Content != null && item.Content !== '') {
                                            st = item.Content;
                                        }
                                   
                                    break;

                                case 'twitter':
                                    d = parseTwitterDate(item.Date);
                                    var un = item.Link.Text, ua = item.Thumb;
                                    href = 'https://www.twitter.com/' + un.split('@')[1];
                                    q = item.Link.Url;
                                    y = '<a href="' + q + '" class="thumb"><img src="' + ua.Src + '" alt="' + ua.Alt + '" /></a>';
                                    z = '<span class="twitter-user"><a href="' + q + '"><strong>' + item.Title + ' </strong>' + un + '</a></span>';
                                    if (item.Content !== null && item.Content !== '') {
                                        z += linkify(item.Content);
                                    }
                                    break;

                              
                                case 'pinterest':
                                    var src = $('img', item.description).attr('src');
                                    if (src) {
                                        var res = src.replace('236x', '736x');
                                    }
                                    y = src ? '<a href="' + q + '"><img src="' + res + '" alt="" /></a>' : '';
                                    z = item.title;
                                    st = z;
                                    break;

                                case 'youtube':
                                    x = item.snippet.title;
                                    var vidId = cq.length > 1 ? item.id.videoId : item.snippet.resourceId.videoId;
                                    var ytthumb = o.thumb == '0' ? 'medium' : o.thumb;
                                    q = 'https://www.youtube.com/watch?v=' + vidId + '&feature=youtube_gdata';
                                    sq = q;
                                    y = '<a href="' + q + '" title="' + item.snippet.title + '"><img src="' + item.snippet.thumbnails[ytthumb].url + '" alt="" /></a>';
                                    z = o.text > 0 ? cut(item.snippet.description, o.text) : item.snippet.description;
                                    d = item.snippet.publishedAt;
                                    var profile = 'Youtube';
                                    if (cq.length > 1) {
                                        profile = decodeURIComponent(id);
                                    } else if (cp.length > 1) {
                                        profile = cp[0];
                                    } else if (cc.length > 1) {
                                        profile = cc[0];
                                    }
                                    u = '<a href="' + href + '">' + profile + '</a>';
                                    break;

                                case 'flickr':
                                    d = parseTwitterDate(item.published);
                                    x = item.title;
                                    y = '<a href="' + q + '" title="' + item.title + '"><img src="' + item.media.m + '" alt="" /></a>';
                                    break;

                               
                                case 'instagram':
                                    d = parseInt(item.created_time * 1000, 10);
                                    x = '';
                                    y = '<a href="' + q + '"><img src="' + item.images[o.thumb].url + '" alt="" /></a>';
                                    z = item.caption != null ? htmlEncode(item.caption.text) : '';
                                    if (item.comments.count > 0 && o.comments > 0) {
                                        i = 0;
                                        m += '<span class="meta"><span class="comments">' + num(item.comments.count) + ' comments</span></span>';
                                        if (item.comments.data !== undefined) {
                                            $.each(item.comments.data, function (i, cmt) {
                                                if (o.comments > i) {
                                                    m += '<span class="meta item-comments"><img src="' + cmt.from.profile_picture + '" />';
                                                    m += cmt.from.full_name + ' - ' + cmt.text + '</span>';
                                                    i++;
                                                } else {
                                                    return false;
                                                }
                                            });
                                        }
                                    }

                                    if (item.likes.count > 0 && o.likes > 0) {
                                        i = 0;
                                        m += '<span class="meta"><span class="likes">' + num(item.likes.count) + ' likes</span></span>';
                                        m += '<span class="meta item-likes">';
                                        if (item.likes.data !== undefined) {
                                            $.each(item.likes.data, function (i, lk) {
                                                if (o.likes > i) {
                                                    m += '<img src="' + lk.profile_picture + '" />';
                                                    i++;
                                                } else {
                                                    return false;
                                                }
                                            });
                                        }
                                        m += '</span>';
                                    }
                                    u = '<a href="' + q + '">' + item.user.username + '</a>';
                                    href = 'https://instagram.com/' + item.user.username;
                                    st = item.caption != null ? item.caption.text : '';
                                    break;
                            }

                            icon = '<a href="' + href + '"><img src="' + o.icon + '" alt="" class="icon" /></a>';

                            if (type == 'twitter') {
                                var intent = 'https://twitter.com/intent/';
                                s = '<a href="' + intent + 'tweet?in_reply_to=' + sq + '&via=' + opt.twitterId + '" class="share-reply"></a>';
                                s += '<a href="' + intent + 'retweet?tweet_id=' + sq + '&via=' + opt.twitterId + '" class="share-retweet"></a>';
                                s += '<a href="' + intent + 'favorite?tweet_id=' + sq + '" class="share-favorite"></a>';
                                s += share('', 'https://twitter.com/' + un + '/status/' + sq, opt.twitterId);
                            } else {
                                s = share(st, sq, opt.twitterId);
                            }

                            $.each(o.out.split(','), function (i, v) {
                                zz += v != 'intro' ? '<span class="section-' + v + '">' : '';
                                switch (v) {
                                    case 'intro':
                                        if (type == 'twitter') {
                                            zintro = '<span class="section-' + v + '"><a href="' + q + '">' + decodeURIComponent(intro) + '</a> <span><a href="https://twitter.com/' + un + '/status/' + item.id_str + '">' + nicetime(new Date(d).getTime(), 0) + '</a></span></span>';
                                        }
                                        else if (type == 'facebook') {
                                            zintro = '';
                                        }
                                        else {
                                            zintro = '<span class="section-' + v + '"><a href="' + q + '">' + decodeURIComponent(intro) + '</a> <span>' + nicetime(new Date(d).getTime(), 0) + '</span></span>';
                                        }
                                        break;
                                    case 'picture':
                                        if (type === 'facebook' && picture !== undefined && picture !== '') {
                                            zz += '<a href="' + q + '"><img src="' + picture.Src + '" alt="' + picture.Alt + '" /></a>';
                                        }
                                        break;
                                    case 'title':
                                        if (type === 'facebook') {
                                            if (thumbnail !== undefined && thumbnail !== '') {
                                                zz += y;
                                            }
                                            if (u !== undefined && u !== '') {
                                                zz += '<a href="' + q + '"><span class="title">' + u + '</span></a>';
                                            }
                                            if (dateloc !== undefined && dateloc !== '') {
                                                zz += '<span class="dateloc">' + dateloc + '</span>';
                                            }
                                        }
                                        else {
                                            zz += x;
                                        }
                                        break;
                                    case 'thumb':
                                        if (type == 'rss') {
                                            var src = item.content.indexOf("img") >= 0 ? $('img', item.content).attr('src') : '';
                                            y = src ? '<a href="' + q + '" class="thumb"><img src="' + src + '" alt="" /></a>' : '';
                                        }
                                        zz += y;
                                        break;
                                    case 'text':
                                        if (type === 'facebook') {
                                            if (st !== undefined && st !== '') {
                                                zz += '<span class="content">' + st + '</span>';
                                            }
                                        }
                                        else {
                                            zz += z;
                                        }
                                        break;
                                    case 'user':
                                        zz += u;
                                        break;
                                    case 'meta':
                                        zz += m;
                                        break;
                                    case 'share':
                                        zz += s;
                                        break;
                                }
                                zz += v != 'intro' ? '</span>' : '';
                            });

                            var df = type == 'instagram' ? nicetime(d, 1) : nicetime(new Date(d).getTime(), 1);
                            var ob = df;
                            switch (opt.order) {
                                case 'random':
                                    ob = randomish(6);
                                    break;
                                case 'none':
                                    ob = 1;
                                    break;
                            }
                            var out = '<li rel="' + ob + '" class="feeds-li feeds-' + type + ' feeds-feed-' + fn + '">' + w + '<div class="inner">' + zz + '<span class="clear"></span></div>' + icon + '</li>', str = decodeURIComponent(opt.remove), rem = q;
                            if (type == 'twitter') {
                                rem = 'https://twitter.com/' + un + '/status/' + item.id_str;
                            }
                            if (str.indexOf(rem) !== -1 && rem != '') {
                                n = n + 1;
                            } else {
                                if (opt.max == 'days') {
                                    if (df <= f2 * 86400 && df >= f1 * 86400) {
                                        list.push(out);
                                    } else if (df > f2 * 86400) {
                                        return false;
                                    }
                                } else {
                                    list.push(out);
                                }
                            }
                        }
                    });
                } else if (opt.debug == true) {
                    list.push('<li class="feeds-li feeds-error">Error. ' + error + '</li>');
                }
            },
            complete: function () {
                var $newItems = $(list.join(''));
                if (opt.wall == true) {
                    stream.isotope('insert', $newItems);
                    if (type == 'facebook' && cp.length < 2) {
                        setTimeout(function () {
                            stream.isotope('layout');
                        }, 1000);
                        $('img', stream).on('load', function () { stream.isotope('layout'); });
                    }
                    if (type == 'twitter') {
                        setTimeout(function () {
                            stream.isotope('layout');
                        }, 1000);
                        $('img', stream).on('load', function () { stream.isotope('layout'); });
                    }
                } else {
                    stream.append($newItems);
                    sortstream(stream, 'asc');
                }
                if (type == 'flickr' && cq.length > 1) {
                    flickrHrefLink(cq[1], $newItems);
                }
            }

        });
        return;
    }

    function linkify(text) {
        text = text.replace(
			/((https?\:\/\/)|(www\.))(\S+)(\w{2,4})(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/gi,
			function (url) {
			    var full_url = !url.match('^https?:\/\/') ? 'http://' + url : url;
			    return '<a href="' + full_url + '">' + url + '</a>';
			}
		);
        text = text.replace(/(^|\s)@(\w+)/g, '$1@<a href="http://www.twitter.com/$2">$2</a>');
        text = text.replace(/(^|\s)#(\w+)/g, '$1#<a href="http://twitter.com/search/%23$2">$2</a>');
        return text;
    }

    function cut(text, n) {
        var short = text.substr(0, n);
        if (/^\S/.test(text.substr(n)))
            short = short.replace(/\s+\S*$/, "");
        return short;
    }

    function htmlEncode(v) {
        return $('<div/>').text(v).html();
    }

    function stripHtml(v) {
        var $html = $(v);
        return $html.text();
    }
        
    function parseTwitterDate(a) {
        var out = !!navigator.userAgent.match(/Trident\/7\./) || navigator.userAgent.indexOf("MSIE") >= 0 ? a.replace(/(\+\S+) (.*)/, '$2 $1') : a;
        return out;
    }

    function nicetime(a, out) {
        var d = Math.round((+new Date - a) / 1000), fuzzy = '', n = 'mins';
        if (out == 1) {
            return d;
        } else if (out == 0) {
            var chunks = new Array();
            chunks[0] = [60 * 60 * 24 * 365, 'year', 'years'];
            chunks[1] = [60 * 60 * 24 * 30, 'month', 'months'];
            chunks[2] = [60 * 60 * 24 * 7, 'week', 'weeks'];
            chunks[3] = [60 * 60 * 24, 'day', 'days'];
            chunks[4] = [60 * 60, 'hr', 'hrs'];
            chunks[5] = [60, 'min', 'mins'];
            var i = 0, j = chunks.length;
            for (i = 0; i < j; i++) {
                s = chunks[i][0];
                if ((xj = Math.floor(d / s)) != 0) {
                    n = xj == 1 ? chunks[i][1] : chunks[i][2];
                    break;
                }
            }
            fuzzy += xj == 1 ? '1 ' + n : xj + ' ' + n;
            if (i + 1 < j) {
                s2 = chunks[i + 1][0];
                if (((xj2 = Math.floor((d - (s * xj)) / s2)) != 0)) {
                    n2 = (xj2 == 1) ? chunks[i + 1][1] : chunks[i + 1][2];
                    fuzzy += (xj2 == 1) ? ' + 1 ' + n2 : ' + ' + xj2 + ' ' + n2;
                }
            }
            fuzzy += ' ago';
            return fuzzy;
        }
    }

    function num(a) {
        var b = a;
        if (a > 999999) b = Math.floor(a / 1E6) + "M";
        else if (a > 9999) b = Math.floor(a / 1E3) + "K";
        else if (a > 999) b = Math.floor(a / 1E3) + "," + a % 1E3;
        return b
    }

    function parseQ(url) {
        var v = [], hash, q = url.split('?')[1];
        if (q != undefined) {
            q = q.split('&');
            for (var i = 0; i < q.length; i++) {
                hash = q[i].split('=');
                v.push(hash[1]);
                v[hash[0]] = hash[1];
            }
        }
        return v;
    }

    function sortstream(obj, d) {
        var $l = $('li', obj);
        $l.sort(function (a, b) {
            var keyA = parseInt($(a).attr('rel'), 10), keyB = parseInt($(b).attr('rel'), 10);
            if (d == 'asc') {
                return (keyA > keyB) ? 1 : -1;
            } else {
                return (keyA < keyB) ? 1 : -1;
            }
            return 0;
        });
        $.each($l, function (index, row) {
            obj.append(row);
        });
        $('.feeds-loading').slideUp().remove();
        return;
    }

    function randomish(l) {
        var i = 0, out = '';
        while (i < l) {
            out += Math.floor((Math.random() * 10) + 1) + '';
            i++;
        }
        return out;
    }

    function ticker(s, b, speed) {
        var $a = $('li:last', s), $b = $('li:first', s), $gx, bh = $b.outerHeight(true);
        if ($('li', s).not('.inactive').length > 2) {
            if (b == 'next') {
                $gx = $a.clone().hide();
                $b.before($gx);
                $a.remove();
                if ($a.hasClass('inactive')) {
                    ticker(s, b, speed);
                } else {
                    $('.inner', $gx).css({ opacity: 0 });
                    $gx.slideDown(speed, 'linear', function () {
                        $('.inner', this).animate({ opacity: 1 }, speed);
                    });
                    return;
                }
            } else {
                $gx = $b.clone();
                if ($b.hasClass('inactive')) {
                    $a.after($gx);
                    $b.remove();
                    ticker(s, b, speed);
                } else {
                    $b.animate({ marginTop: -bh + 'px' }, speed, 'linear', function () {
                        $a.after($gx);
                        $b.remove();
                    });
                    $('.inner', $b).animate({ opacity: 0 }, speed);
                }
            }
        }
    }

    function flickrHrefLink(id, obj) {
        jQuery.ajax({
            url: 'http://api.flickr.com/services/feeds/groups_pool.gne?id=' + id + '&format=json&jsoncallback=?',
            dataType: 'jsonp',
            success: function (a) {
                $('.icon', obj).each(function () {
                    $(this).parent().attr('href', a.link);
                });
            }
        });
    }

    function share(st, sq, twitterId) {
        var s = '', sq = encodeURIComponent(sq), st = encodeURIComponent(st);
        s = '<a href="http://www.facebook.com/sharer.php?u=' + sq + '&t=' + st + '" class="share-facebook" target="_blank"></a>';
        s += '<a href="https://twitter.com/share?url=' + sq + '&text=' + st + '&via=' + twitterId + '" class="share-twitter" target="_blank"></a>';
        s += '<a href="https://plus.google.com/share?url=' + sq + '" class="share-google" target="_blank"></a>';
        s += '<a href="http://www.linkedin.com/shareArticle?mini=true&url=' + sq + '&title=' + st + '" class="share-linkedin" target="_blank"></a>';
        return s;
    }
})(jQuery);

jQuery(window).load(function () {
    jQuery.getScript("//platform.twitter.com/widgets.js", function () { });
    jQuery('.section-share a').click(function () {
        var u = jQuery(this).attr('href');
        window.open(u, 'sharer', 'toolbar=0,status=0,width=626,height=436');
        return false;
    });

    var filters = {}, $container = jQuery('.stream');

    jQuery('.filter a').click(function () {
        var $i = jQuery(this), isoFilters = [], prop, selector, $a = $i.parents('.feeds-toolbar'), $b = $a.next(), $c = jQuery('.stream', $b);

        jQuery('.filter a', $a).removeClass('iso-active');
        $i.addClass('iso-active');
        filters[$i.data('group')] = $i.data('filter');
        for (prop in filters) {
            isoFilters.push(filters[prop])
        }
        selector = isoFilters.join('');
        $c.isotope({ filter: selector, sortBy: 'postDate' });

        return false;
    });

    jQuery.each($container, function () {
        jQuery('li .section-thumb img, li .section-text img', jQuery(this)).css('opacity', 0).show().fadeTo(800, 1);
        jQuery(this).isotope('layout');
    });

    function sortstream(obj, d) {
        var $l = jQuery('li.feeds-li', obj);
        $l.sort(function (a, b) {
            var keyA = parseInt(jQuery(a).attr('rel'), 10), keyB = parseInt(jQuery(b).attr('rel'), 10);
            if (d == 'asc') { return (keyA > keyB) ? 1 : -1; }
            else { return (keyA < keyB) ? 1 : -1; }
            return 0;
        });
        jQuery.each($l, function (index, row) {
            obj.append(row);
        });
        return;
    }
});
