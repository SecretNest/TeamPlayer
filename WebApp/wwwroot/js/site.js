// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/**
 * 对元素的索引占位符进行替换。
 * @param {any} ele
 * @param {any} index
 */
function replaceArrayIndex(ele, index) {

	var name = $(ele).attr('name');

	if (name) {
		var newName = name.replace('[0]', '[' + index + ']');
		$(ele).attr('name', newName);
	}

}

/**
 * 重新排列表单中需要编号的部分。
 * @param {any} form
 */
function reorderData(form) {

	$('tbody tr', form).each(function (index, row) {
		$(':input', row).each(function (eleIndex, ele) {
			replaceArrayIndex(ele, index);
		});

	});
}
