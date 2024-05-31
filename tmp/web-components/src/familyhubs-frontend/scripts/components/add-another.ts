// A version of the MOJ's add-another component that plays nice with the accessible autocomplete component
// and works with errored fields.
// I did consider subclassing the MOJ's add-another component,
// but it would have been so coupled that it would've probably broken on an update of the MOJ library.
// So instead we forked it and made our own version.

//todo: when accessible-autocomplete creates the input, it doesn't handle the aria-describedby correctly...
// https://github.com/alphagov/accessible-autocomplete/issues/589

window.FamilyHubsFrontend = window.FamilyHubsFrontend || {};

export function initializeAddAnother(): void {
    //todo: support options with scope?
    var $addAnothers = document.querySelectorAll('[data-module="fh-add-another"]');

	$addAnothers.forEach(function ($addAnother) {
		new window.FamilyHubsFrontend.AddAnother($addAnother);
    });
}

window.FamilyHubsFrontend.AddAnother = function (container) {
	this.container = $(container);
	this.index = 0;

	if (this.container.data('fh-add-another-initialised')) {
		return
	}

	//todo: this is a bit hacky - find a better way to do this
	var functionName = container.getAttribute('data-fh-add-another-callback');

	this.callback = null;
	document.addEventListener('DOMContentLoaded', function () {
		if (typeof window[functionName] === 'function') {
			this.callback = window[functionName];
			this.callback(container);
		}
	}.bind(this));

	this.container.data('fh-add-another-initialised', true);

	this.container.on('click', '.fh-add-another__remove-button', $.proxy(this, 'onRemoveButtonClick'));
	this.container.on('click', '.fh-add-another__add-button', $.proxy(this, 'onAddButtonClick'));
	this.container.find('.fh-add-another__add-button, fh-add-another__remove-button').prop('type', 'submit');
};

window.FamilyHubsFrontend.AddAnother.prototype.onAddButtonClick = function (e) {
	var item = this.getNewItem();

	var firstItem = this.getItems().first();
	if (!this.hasRemoveButton(firstItem)) {
		this.createRemoveButton(firstItem);
	}
	this.getItems().last().after(item);
	item.find('input, textarea, select').first().focus();
	e.preventDefault();
};

window.FamilyHubsFrontend.AddAnother.prototype.hasRemoveButton = function (item) {
	return item.find('.fh-add-another__remove-button').length;
};

window.FamilyHubsFrontend.AddAnother.prototype.getItems = function () {
	return this.container.find('.fh-add-another__item');
};

// todo: ? a better approach would be to have a template item that we clone,
// rather than having to strip the error from the first item
window.FamilyHubsFrontend.AddAnother.prototype.stripErrorFromNewItem = function (item: HTMLElement) {

	// remove the govuk-form-group--error class from any divs that have it set
	item.querySelectorAll('div.govuk-form-group--error').forEach(function (el, index) {
		el.classList.remove('govuk-form-group--error');
	});

	// find all paragraphs with the class govuk-error-message and remove them
	item.querySelectorAll('p.govuk-error-message').forEach(function (el, index) {
        el.parentNode.removeChild(el);
	});

	// remove the govuk-select--error class from any selects that have it set
	item.querySelectorAll('select.govuk-select--error').forEach(function (el, index) {
        el.classList.remove('govuk-select--error');
	});

	// remove the govuk-input--error class from any inputs that have it set
	item.querySelectorAll('input.govuk-input--error').forEach(function (el, index) {
		el.classList.remove('govuk-input--error');
	});
}

window.FamilyHubsFrontend.AddAnother.prototype.getNewItem = function () { //: JQuery<HTMLElement> //HTMLElement {
    // get the first item and clone it
    const items = this.getItems();
    const item = items[0].cloneNode(true) as HTMLElement;

	this.stripErrorFromNewItem(item);

    // find the autocomplete wrappers and remove the elements that are added by accessible-autocomplete
    const autocompleteWrappers = item.querySelectorAll('.autocomplete__wrapper');
    autocompleteWrappers.forEach(wrapper => {
        if (wrapper.parentNode.parentNode) {
			wrapper.parentNode.parentNode.removeChild(wrapper.parentNode);
        }
    });

	var $item = $(item);

	// update the id and name attributes
	this.updateAttributes(++this.index, $item);

	this.resetItem($item);

	// call the callback which needs to apply accessibility-autocomplete enhancements to the new item
	if (typeof this.callback === 'function') {
		this.callback(item);
	}

    // Create a remove button if it doesn't exist
    if (!this.hasRemoveButton($item)) {
        this.createRemoveButton($item);
    }

    return $item;
};

window.FamilyHubsFrontend.AddAnother.prototype.updateAttributes = function (index, item) {
	item.find('[data-name]').each(function (i, el) {
		var originalId = el.id

		el.name = $(el).attr('data-name').replace(/%index%/, index);
		el.id = $(el).attr('data-id').replace(/%index%/, index);

		var label = $(el).siblings('label')[0] || $(el).parents('label')[0] || item.find('[for="' + originalId + '"]')[0];
		label.htmlFor = el.id;
	});
};

window.FamilyHubsFrontend.AddAnother.prototype.createRemoveButton = function (item) {
	item.append('<button type="submit" class="govuk-button govuk-button--secondary fh-add-another__remove-button">Remove</button>');
};

window.FamilyHubsFrontend.AddAnother.prototype.resetItem = function (item) {
	// accessibile-autocomplete adds an input (without data-name or data-id)
	// so we blank all input controls
    item.find('input, textarea, select').each(function (index, el) {
		if (el.type == 'checkbox' || el.type == 'radio') {
			el.checked = false;
		}
		else {
            el.value = '';
        }
    });
};

window.FamilyHubsFrontend.AddAnother.prototype.onRemoveButtonClick = function (e) {
	$(e.currentTarget).parents('.fh-add-another__item').remove();
	var items = this.getItems();
	if (items.length === 1) {
		items.find('.fh-add-another__remove-button').remove();
	}
	this.focusHeading();
	e.preventDefault();
};

window.FamilyHubsFrontend.AddAnother.prototype.focusHeading = function () {
	this.container.find('.fh-add-another__heading').focus();
};
