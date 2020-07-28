'use strict';

const CONSTANTS = {
    ID_LIST: 'file_manager_list',
    CLASS_LIST_NAME: 'file-manager-list',
    CLASS_DISABLE_SORT_ITEM: 'file-manager-disable-sort-item',
    CLASS_ADD_IMAGE_BUTTON: 'file-manager-add-image-button',
    CLASS_UNSORTABLE: 'file-manager-unsortable',
    CLASS_DELETE_IMAGE_BUTTON: 'file-manager-delete-image-button',
    CLASS_FILE_IMAGE: 'file-manager-image',
    CLASS_MAIN_IMAGE_BUTTON: 'file-manager-main-image-button',
    CLASS_MAIN_IMAGE: 'file-manager-main-image'

};



const STATES = {
    NOCHANGES: 'nochanges',
    ADDED: 'added',
    MODIFIED: 'modified',
    DELETED: 'deleted'
};

const INPUT_TYPE = {
    FILE: 'file'
};

const VISIBILITY = {
    VISIBLE: 'visible',
    HIDDEN: 'hidden',
    COLLAPSE: 'collapse',
    INITIAL: 'initial',
    INHERIT: 'inherit'
};

const DISPLAY = {
    NONE: 'none',
    INITIAL: 'initial',
    BLOCK: 'block'

};

var getElemById = (id) => document.getElementById(id);

var extendHTMLElement = function (functionName, callBack) { Object.defineProperty(HTMLElement.prototype, functionName, { value: callBack }); }
extendHTMLElement('setElementId', function (value) { this.setAttribute('id', value); return this; });
extendHTMLElement('setId', function (value) { this.setAttribute('data-id', value); return this; });
extendHTMLElement('setState', function (value) { this.setAttribute('data-state', value); return this; });
extendHTMLElement('setFileName', function (value) { this.setAttribute('data-file-name', value); return this; });
extendHTMLElement('setBackground', function (value) {
    if (value == null) {
        this.style.background = null;
    }
    else if (value instanceof HTMLElement) {
        var self = this;
        getBase64Image(value, (base64) => self.style.background = 'url(' + base64 + ')');
    }
    else if (value.includes('base64')) {
        this.style.background = 'url(' + value + ')';
    } else {
    }
    return this;
});
extendHTMLElement('setType', function (value) { this.type = value; return this; });
extendHTMLElement('addElementClass', function (value) { this.className += ' ' + value; this.className = this.className.trim(); return this; });
extendHTMLElement('removeElementClass', function (value) { this.classList.remove(value); return this; });
extendHTMLElement('onClick', function (value) { this.addEventListener('click', value); return this; });
extendHTMLElement('hideElement', function (value) { this.style.display = DISPLAY.NONE; return this; });
extendHTMLElement('showElement', function (value) { this.style.display = DISPLAY.INITIAL; return this; });
extendHTMLElement('setSize', function (value) { this.style.height = value.height + 'px'; this.style.width = value.width + 'px'; return this; });
extendHTMLElement('setLeft', function (value) { this.style.left = value + 'px'; return this; });

Object.defineProperty(Object.prototype, 'filemanagerjs', {
    value: function (options) {
        var elem = {};
        if ($ && this instanceof $) {
            elem = this.get(0);
        } else {
            elem = this;
        }
        if (!elem.filemanager) {
            elem.filemanager = new FileManager(elem, options);
        }
        return elem.filemanager;
    }
});

var createElement = (elem) => document.createElement(elem);
var createText = (text) => document.createTextNode(text);

var createList = () => createElement('ul');
var createListItem = () => createElement('li');
var createSpan = () => createElement('span');
var createInput = (type) => createElement('input').setType(type);
var createDiv = () => createElement('div');
var createLabel = () => createElement('label');

class FileManagerOptions {
    constructor() {
        this.multipleFiles = true;
        this.sortable = true;
        this.width = 200;
        this.height = 200;
        this.UseMainImage = false;
    }
    getSize() {
        return {
            height: this.height,
            width: this.width
        };
    }
}



class FileManager {

    constructor(obj, options) {
        if (!options) {
            this.options = new FileManagerOptions();
        }
        if ($ && obj instanceof $) {
            this.container = obj.get(0);
        }
        else if (obj instanceof HTMLElement) {
            this.container = obj;
        }
        else if (typeof obj == 'string') {
            this.container = getElemById(obj);
        }
        if (!this.container) {
            console.error('FileManagerJS:', 'container not found');
            return;
        }
        this.options = options;
        var images = [];
        var length = this.container.children.length;
        for (var i = 0; i < length; i++) {
            images.push(this.container.children[0]);
            this.container.children[0].remove();
        }
        this.listOfId = {};
        this.mainListItem = null;
        this.createList();
        this.createAddButtom();
        this.initSortable();
        this.initMultipleFiles();
        this.initList(images);
    }

    initSortable() {
        if (this.options.sortable) {
            $(this.list).sortable(
                {
                    items: 'li:not(.' + CONSTANTS.CLASS_UNSORTABLE + ')',
                    cancel: '.' + CONSTANTS.CLASS_DISABLE_SORT_ITEM,
                    stop: (event, ui) => {
                    }
                });
            $(this.list).disableSelection();
        }
    }

    initMultipleFiles() {
        if (this.options.multipleFiles) {
            this.fileInput.setAttribute('multiple', '');
        }
    }

    createList() {
        this.list = createList();
        this.listOfId[CONSTANTS.ID_LIST] = CONSTANTS.ID_LIST + '_' + NewGuid();
        this.list.setElementId(this.listOfId[CONSTANTS.ID_LIST]);
        this.list.addElementClass(CONSTANTS.CLASS_LIST_NAME);
        this.container.appendChild(this.list);
    }

    initList(images) {
        for (var i = 0; i < images.length; i++) {
            var img = this.addImage(images[i]).setState(STATES.NOCHANGES);
            var data = images[i].dataset;
            var keys = Object.keys(data);
            for (var j = 0; j < keys.length; j++) {
                img.dataset[keys[j]] = data[keys[j]];
            }
            if (!this.options.multipleFiles) {
                this.singleImage = img;
                this.addButton.hideElement();
                return;
            }
            if (data['isMain'] == 'True') {
                this.setAsMain(img);
            }
        }
    }

    createAddButtom() {
        this.addButton = createListItem()
            .addElementClass(CONSTANTS.CLASS_ADD_IMAGE_BUTTON)
            .addElementClass(CONSTANTS.CLASS_DISABLE_SORT_ITEM)
            .addElementClass(CONSTANTS.CLASS_UNSORTABLE)
            .setSize(this.options.getSize());
        var label = createLabel();
        var span = createDiv();
        this.fileInput = createInput(INPUT_TYPE.FILE)
            .hideElement();
        var self = this;
        this.fileInput.addEventListener("change", function () {
            for (var i = 0; i < this.files.length; i++) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = self.addImage(e.target.result, e.target.file.name);
                    if (!self.options.multipleFiles) {
                        if (!self.singleImage) {
                            self.singleImage = img;
                            self.singleImage.setId(DefauldGuid());
                            img.setState(STATES.ADDED);
                            self.addButton.hideElement();
                        }
                        var state = self.singleImage.dataset['state'];
                        if (state == STATES.NOCHANGES) {
                            self.singleImage.setState(STATES.MODIFIED);
                        } else if (state == STATES.DELETED) {
                            self.singleImage.setState(STATES.MODIFIED);
                        }
                    } else {
                        img.setState(STATES.ADDED).setId(DefauldGuid()).setId(DefauldGuid());
                        if (!self.mainListItem) {
                            self.setAsMain(img);
                        } else {
                            img.dataset['isMain'] = false; 
                        }

                    }
                };
                reader.file = this.files[i];
                reader.readAsDataURL(this.files[i]);
            }
        });
        this.fileInput;
        label.appendChild(span);
        label.appendChild(this.fileInput);
        this.addButton.appendChild(label);
        this.list.appendChild(this.addButton);
    }

    addImage(image, fileName) {
        var listItem = {};
        var self = this;
        var div = {};
        if (!this.options.multipleFiles && this.singleImage) {
            this.addButton.hideElement();
            this.singleImage.showElement();
            div = this.singleImage.children[1];
            listItem = this.singleImage;
            listItem.setFileName(fileName);

        } else {
            listItem = createListItem()
                .addElementClass(CONSTANTS.CLASS_FILE_IMAGE)
                .setFileName(fileName);
            div = createDiv()
                .setSize(this.options.getSize());
            listItem.appendChild(createSpan()
                .setLeft(this.options.width - 30)
                .addElementClass(CONSTANTS.CLASS_DELETE_IMAGE_BUTTON)
                .onClick(function () {
                    self.removeItem(listItem);
                }));
            if (this.options.UseMainImage) {
                listItem.appendChild(createSpan()
                    .setLeft(this.options.width - 70)
                    .addElementClass(CONSTANTS.CLASS_MAIN_IMAGE_BUTTON)
                    .addElementClass('glyphicon glyphicon-home')
                    .onClick(function () {
                        self.setAsMain(listItem);
                    }));
            }
            listItem.appendChild(div);
            if (!this.options.multipleFiles) {
                div.onClick(function () {
                    self.fileInput.click();
                });
            }
            this.list.insertBefore(listItem, this.list.children[this.list.children.length - 1]);
        }

        div.setBackground(image);

        var self = this;
        if (image instanceof HTMLElement) {
            var data = image.dataset;
            var keys = Object.keys(data);
            for (var j = 0; j < keys.length; j++) {
                listItem.dataset[keys[j]] = data[keys[j]];
            }
        }
        return listItem;
    }

    setAsMain(listItem) {
        if (this.mainListItem) {
            var oldItem = this.mainListItem.children[1];
            oldItem.classList.remove(CONSTANTS.CLASS_MAIN_IMAGE_BUTTON);
            this.mainListItem.dataset['isMain'] = false;
            this.mainListItem.classList.remove(CONSTANTS.CLASS_MAIN_IMAGE);
            if (this.mainListItem.dataset['state'] != STATES.ADDED && this.mainListItem.dataset['state'] != STATES.DELETED) {
                this.mainListItem.dataset['state'] = STATES.MODIFIED;
            }
            if (listItem.dataset['state'] != STATES.ADDED && listItem.dataset['state'] != STATES.DELETED) {
                listItem.dataset['state'] = STATES.MODIFIED;
            }
            oldItem.addElementClass(CONSTANTS.CLASS_MAIN_IMAGE_BUTTON);
        }
        listItem.dataset['isMain'] = true;
        listItem.addElementClass(CONSTANTS.CLASS_MAIN_IMAGE);
        this.mainListItem = listItem;
    }

    removeItem(listItem) {
        if (listItem.dataset['state'] == STATES.ADDED) {
            listItem.remove();
            if (!this.options.multipleFiles) {
                this.singleImage = null;
            }
        } else {
            listItem.setBackground(null)
                .setState(STATES.DELETED)
                .hideElement();
        }

        if (!this.options.multipleFiles) {
            this.addButton.showElement();
        }
    }

    getFiles() {
        var files = [];
        for (var i = 0; i < this.list.children.length; i++) {
            var elem = this.list.children[i];
            if (elem.className.includes(CONSTANTS.CLASS_FILE_IMAGE)) {
                var data = elem.dataset;
                var keys = Object.keys(data);
                var obj = {};
                for (var j = 0; j < keys.length; j++) {
                    obj[keys[j]] = data[keys[j]];
                }
                obj['file'] = dataURLtoFile(elem.children[elem.children.length - 1].style.background.substring(5, elem.children[elem.children.length - 1].style.background.length - 2), data.fileName);
                files.push(obj);
            }

        }
        return files;
    }

}

function dataURLtoFile(dataurl, filename) {
    if (dataurl == null) {
        return null;
    }
    var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }
    return new File([u8arr], filename, { type: mime });
}

function getBase64Image(img, callback) {
    if (img.tagName != 'IMG') {
        return;
    }
    img.onload = () => {
        var canvas = document.createElement("canvas");
        canvas.width = img.width;
        canvas.height = img.height;
        var ctx = canvas.getContext("2d");
        ctx.drawImage(img, 0, 0);
        var dataURL = canvas.toDataURL("image/png");
        callback(dataURL);
    }
}
function DefauldGuid() {
    return '00000000-0000-0000-0000-000000000000';
}

function NewGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}