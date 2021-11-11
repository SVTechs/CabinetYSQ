var lookup = {};

var onTreeItemClick = function (record, e) {
    if (record.isLeaf()) {
        e.stopEvent();
        loadPage(record.get('url'), record.getId(), record.get('text'));
    } else {
        record[record.isExpanded() ? 'collapse' : 'expand']();
    }
};

var loadPage = function (href, id, title) {
    var dest = href;
    if (dest.substring(0, 3) === "nw:") {
        window.open(dest.substring(3));
        return;
    }
    var tab = App.MainTabs.getComponent(id),
        lObj = lookup[href];

    var slashFields = href.split("/");
    if (slashFields.length > 4) {
        href = "";
        for (var i in [0, 1, 2, 3]) {
            href = href + slashFields[i] + "/"
        }
    } else if (href[href.length - 1] != "/") {
        href = href + "/";
    }

    if (id == "-") {
        App.direct.GetHashCode(href, {
            success: function (result) {
                loadPage(href, "e" + result, title);
            }
        });

        return;
    }

    lookup[href] = id;

    if (tab) {
        App.MainTabs.setActiveTab(tab);
    } else {
        if (Ext.isEmpty(title)) {
            var m = /(\w+)\/$/g.exec(href);
            title = m == null ? "[No Name]" : m[1];
        }

        title = title.replace(/_/g, " ");
        makeTab(id, dest, title);
    }
};

var makeTab = function (id, url, title) {
    var win,
        tab,
        hostName,
        exampleName,
        node = App.menuTree.getStore().getNodeById(id),
        tabTip;

    if (id === "-") {
        id = Ext.id(undefined, "extnet");
        lookup[url] = id;
    }

    tabTip = url.replace(/^\//g, "");
    tabTip = tabTip.replace(/\/$/g, "");
    tabTip = tabTip.replace(/\//g, " > ");
    tabTip = tabTip.replace(/_/g, " ");

    tab = App.MainTabs.add(new Ext.Panel({
        id: id,
        title: title,
        tabTip: tabTip,
        hideMode: "offsets",

        loader: {
            scripts: true,
            url: url,
            renderer: "frame",
            listeners: {
                beforeload: function () {
                    this.target.body.mask('Loading...');
                },
                load: {
                    fn: function (loader) {
                        this.target.body.unmask();
                    }
                }
            }
        },
        listeners: {
            deactivate: {
                fn: function (el) {
                    if (this.sWin && this.sWin.isVisible()) {
                        this.sWin.hide();
                    }
                }
            },

            destroy: function () {
                if (this.sWin) {
                    this.sWin.close();
                    this.sWin.destroy();
                }
            }
        },
        closable: true
    }));

    tab.sWin = win;
    setTimeout(function () {
            App.MainTabs.setActiveTab(tab);
        },
        250);

    var expandAndSelect = function (node) {
        var view = App.menuTree.getView(),
            originalAnimate = view.animate;

        view.animate = false;
        node.bubble(function (node) {
            node.expand(false);
        });

        App.menuTree.getSelectionModel().select(node);
        view.animate = originalAnimate;
    };

    if (node) {
        expandAndSelect(node);
        createTagItems(tab, node);
    } else {
        App.menuTree.on("load",
            function (node) {
                node = App.menuTree.getStore().getNodeById(id);
                if (node) {
                    expandAndSelect(node);
                    createTagItems(tab, node);
                }
            },
            this,
            { delay: 10, single: true });
    }
};

var createTagItems = function (tab, node) {

};