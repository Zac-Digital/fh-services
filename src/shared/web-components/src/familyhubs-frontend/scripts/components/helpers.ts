/*todo: remove this and use NodeList.prototype.forEach instead, as govuk has for v5 */
/* https://github.com/alphagov/govuk-frontend/issues/3466 */
/* https://github.com/alphagov/govuk-frontend/pull/3570/files */

export function nodeListForEach(
    nodes: NodeListOf<Element>,
    callback: (node: Element, index: number, nodes: NodeListOf<Element>) => void
): void {
    if (window.NodeList.prototype.forEach) {
        return nodes.forEach(callback);
    }
    for (var i = 0; i < nodes.length; i++) {
        callback.call(window, nodes[i], i, nodes);
    }
}
