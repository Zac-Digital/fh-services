// from https://github.com/ideal-postcodes/postcode
/**
 * @hidden
 */
export const DISTRICT_SPLIT_REGEX = /^([a-z]{1,2}\d)([a-z])$/i;
/**
 * Tests for the unit section of a postcode
 */
export const UNIT_REGEX = /[a-z]{2}$/i;
/**
 * Tests for the inward code section of a postcode
 */
export const INCODE_REGEX = /\d[a-z]{2}$/i;
/**
 * Tests for the outward code section of a postcode
 */
export const OUTCODE_REGEX = /^[a-z]{1,2}\d[a-z\d]?$/i;
/**
 * Tests for a valid postcode
 */
export const POSTCODE_REGEX = /^[a-z]{1,2}\d[a-z\d]?\s*\d[a-z]{2}$/i;
/**
 * Test for a valid postcode embedded in text
 */
export const POSTCODE_CORPUS_REGEX = /[a-z]{1,2}\d[a-z\d]?\s*\d[a-z]{2}/gi;
/**
 * Tests for the area section of a postcode
 */
export const AREA_REGEX = /^[a-z]{1,2}/i;
/**
 * Invalid postcode prototype
 * @hidden
 */
const invalidPostcode = {
    valid: false,
    postcode: null,
    incode: null,
    outcode: null,
    area: null,
    district: null,
    subDistrict: null,
    sector: null,
    unit: null,
};
/**
 * Return first elem of input is RegExpMatchArray or null if input null
 * @hidden
 */
const firstOrNull = (match) => {
    if (match === null)
        return null;
    return match[0];
};
const SPACE_REGEX = /\s+/gi;
/**
 * Drop all spaces and uppercase
 * @hidden
 */
const sanitize = (s) => s.replace(SPACE_REGEX, "").toUpperCase();
/**
 * Sanitizes string and returns regex matches
 * @hidden
 */
const matchOn = (s, regex) => sanitize(s).match(regex);
/**
 * Detects a "valid" postcode
 * - Starts and ends on a non-space character
 * - Any length of intervening space is allowed
 * - Must conform to one of following schemas:
 *  - AA1A 1AA
 *  - A1A 1AA
 *  - A1 1AA
 *  - A99 9AA
 *  - AA9 9AA
 *  - AA99 9AA
 */
export const isValid = (postcode) => postcode.match(POSTCODE_REGEX) !== null;
/**
 * Returns true if string is a valid outcode
 */
export const validOutcode = (outcode) => outcode.match(OUTCODE_REGEX) !== null;
/**
 * Returns a normalised postcode string (i.e. uppercased and properly spaced)
 *
 * Returns null if invalid postcode
 */
export const toNormalised = (postcode) => {
    const outcode = toOutcode(postcode);
    if (outcode === null)
        return null;
    const incode = toIncode(postcode);
    if (incode === null)
        return null;
    return `${outcode} ${incode}`;
};
/**
 * Returns a correctly formatted outcode given a postcode
 *
 * Returns null if invalid postcode
 */
export const toOutcode = (postcode) => {
    if (!isValid(postcode))
        return null;
    return sanitize(postcode).replace(INCODE_REGEX, "");
};
/**
 * Returns a correctly formatted incode given a postcode
 *
 * Returns null if invalid postcode
 */
export const toIncode = (postcode) => {
    if (!isValid(postcode))
        return null;
    const match = matchOn(postcode, INCODE_REGEX);
    return firstOrNull(match);
};
/**
 * Returns a correctly formatted area given a postcode
 *
 * Returns null if invalid postcode
 */
export const toArea = (postcode) => {
    if (!isValid(postcode))
        return null;
    const match = matchOn(postcode, AREA_REGEX);
    return firstOrNull(match);
};
/**
 * Returns a correctly formatted sector given a postcode
 *
 * Returns null if invalid postcode
 */
export const toSector = (postcode) => {
    const outcode = toOutcode(postcode);
    if (outcode === null)
        return null;
    const incode = toIncode(postcode);
    if (incode === null)
        return null;
    return `${outcode} ${incode[0]}`;
};
/**
 * Returns a correctly formatted unit given a postcode
 *
 * Returns null if invalid postcode
 */
export const toUnit = (postcode) => {
    if (!isValid(postcode))
        return null;
    const match = matchOn(postcode, UNIT_REGEX);
    return firstOrNull(match);
};
/**
 * Returns a correctly formatted district given a postcode
 *
 * Returns null if invalid postcode
 *
 * @example
 *
 * ```
 * toDistrict("AA9 9AA") // => "AA9"
 * toDistrict("A9A 9AA") // => "A9"
 * ```
 */
export const toDistrict = (postcode) => {
    const outcode = toOutcode(postcode);
    if (outcode === null)
        return null;
    const match = outcode.match(DISTRICT_SPLIT_REGEX);
    if (match === null)
        return outcode;
    return match[1];
};
/**
 * Returns a correctly formatted subdistrict given a postcode
 *
 * Returns null if no subdistrict is available on valid postcode
 * Returns null if invalid postcode
 *
 * @example
 *
 * ```
 * toSubDistrict("AA9A 9AA") // => "AA9A"
 * toSubDistrict("A9A 9AA") // => "A9A"
 * toSubDistrict("AA9 9AA") // => null
 * toSubDistrict("A9 9AA") // => null
 * ```
 */
export const toSubDistrict = (postcode) => {
    const outcode = toOutcode(postcode);
    if (outcode === null)
        return null;
    const split = outcode.match(DISTRICT_SPLIT_REGEX);
    if (split === null)
        return null;
    return outcode;
};
/**
 * Returns a ValidPostcode or InvalidPostcode object from a postcode string
 *
 * @example
 *
 * ```
 * import { parse } from "postcode";
 *
 * const {
 * postcode,    // => "SW1A 2AA"
 * outcode,     // => "SW1A"
 * incode,      // => "2AA"
 * area,        // => "SW"
 * district,    // => "SW1"
 * unit,        // => "AA"
 * sector,      // => "SW1A 2"
 * subDistrict, // => "SW1A"
 * valid,       // => true
 * } = parse("Sw1A     2aa");
 *
 * const {
 * postcode,    // => null
 * outcode,     // => null
 * incode,      // => null
 * area,        // => null
 * district,    // => null
 * unit,        // => null
 * sector,      // => null
 * subDistrict, // => null
 * valid,       // => false
 * } = parse("    Oh no, ):   ");
 * ```
 */
export const parse = (postcode) => {
    if (!isValid(postcode))
        return Object.assign({}, invalidPostcode);
    return {
        valid: true,
        postcode: toNormalised(postcode),
        incode: toIncode(postcode),
        outcode: toOutcode(postcode),
        area: toArea(postcode),
        district: toDistrict(postcode),
        subDistrict: toSubDistrict(postcode),
        sector: toSector(postcode),
        unit: toUnit(postcode),
    };
};
/**
 * Searches a body of text for postcode matches
 *
 * Returns an empty array if no match
 *
 * @example
 *
 * ```
 * // Retrieve valid postcodes in a body of text
 * const matches = match("The PM and her no.2 live at SW1A2aa and SW1A 2AB"); // => ["SW1A2aa", "SW1A 2AB"]
 *
 * // Perform transformations like normalisation postcodes using `.map` and `toNormalised`
 * matches.map(toNormalised); // => ["SW1A 2AA", "SW1A 2AB"]
 *
 * // No matches yields empty array
 * match("Some London outward codes are SW1A, NW1 and E1"); // => []
 * ```
 */
export const match = (corpus) => corpus.match(POSTCODE_CORPUS_REGEX) || [];
/**
 * Replaces postcodes in a body of text with a string
 *
 * By default the replacement string is empty string `""`
 *
 * @example
 *
 * ```
 * // Replace postcodes in a body of text
 * replace("The PM and her no.2 live at SW1A2AA and SW1A 2AB");
 * // => { match: ["SW1A2AA", "SW1A 2AB"], result: "The PM and her no.2 live at  and " }
 *
 * // Add custom replacement
 * replace("The PM lives at SW1A 2AA", "Downing Street");
 * // => { match: ["SW1A 2AA"], result: "The PM lives at Downing Street" };
 *
 * // No match
 * replace("Some London outward codes are SW1A, NW1 and E1");
 * // => { match: [], result: "Some London outward codes are SW1A, NW1 and E1" }
 * ```
 */
export const replace = (corpus, replaceWith = "") => ({
    match: match(corpus),
    result: corpus.replace(POSTCODE_CORPUS_REGEX, replaceWith),
});
export const FIXABLE_REGEX = /^\s*[a-z01]{1,2}[0-9oi][a-z\d]?\s*[0-9oi][a-z01]{2}\s*$/i;
/**
 * Attempts to fix and clean a postcode. Specifically:
 * - Performs character conversion on obviously wrong and commonly mixed up letters (e.g. O => 0 and vice versa)
 * - Trims string
 * - Properly adds space between outward and inward codes
 *
 * If the postcode cannot be coerced into a valid format, the original string is returned
 *
 * @example
 * ```javascript
 * fix(" SW1A  2AO") => "SW1A 2AO" // Properly spaces
 * fix("SW1A 2A0") => "SW1A 2AO" // 0 is coerced into "0"
 * ```
 *
 * Aims to be used in conjunction with parse to make postcode entry more forgiving:
 *
 * @example
 * ```javascript
 * const { inward } = parse(fix("SW1A 2A0")); // inward = "2AO"
 * ```
 */
export const fix = (s) => {
    const match = s.match(FIXABLE_REGEX);
    if (match === null)
        return s;
    s = s.toUpperCase().trim().replace(/\s+/gi, "");
    const l = s.length;
    const inward = s.slice(l - 3, l);
    return `${coerceOutcode(s.slice(0, l - 3))} ${coerce("NLL", inward)}`;
};
const toLetter = {
    "0": "O",
    "1": "I",
};
const toNumber = {
    O: "0",
    I: "1",
};
const coerceOutcode = (i) => {
    if (i.length === 2)
        return coerce("LN", i);
    if (i.length === 3)
        return coerce("L??", i);
    if (i.length === 4)
        return coerce("LLN?", i);
    return i;
};
/**
 * Given a pattern of letters, numbers and unknowns represented as a sequence
 * of L, Ns and ? respectively; coerce them into the correct type given a
 * mapping of potentially confused letters
 *
 * @hidden
 *
 * @example coerce("LLN", "0O8") => "OO8"
 */
const coerce = (pattern, input) => input
    .split("")
    .reduce((acc, c, i) => {
    const target = pattern.charAt(i);
    if (target === "N")
        acc.push(toNumber[c] || c);
    if (target === "L")
        acc.push(toLetter[c] || c);
    if (target === "?")
        acc.push(c);
    return acc;
}, [])
    .join("");

//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbImNvbXBvbmVudHMvcG9zdGNvZGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsbURBQW1EO0FBRW5EOztHQUVHO0FBQ0gsTUFBTSxDQUFDLE1BQU0sb0JBQW9CLEdBQUcsMEJBQTBCLENBQUM7QUFDL0Q7O0dBRUc7QUFDSCxNQUFNLENBQUMsTUFBTSxVQUFVLEdBQUcsWUFBWSxDQUFDO0FBQ3ZDOztHQUVHO0FBQ0gsTUFBTSxDQUFDLE1BQU0sWUFBWSxHQUFHLGNBQWMsQ0FBQztBQUMzQzs7R0FFRztBQUNILE1BQU0sQ0FBQyxNQUFNLGFBQWEsR0FBRyx5QkFBeUIsQ0FBQztBQUN2RDs7R0FFRztBQUNILE1BQU0sQ0FBQyxNQUFNLGNBQWMsR0FBRyxzQ0FBc0MsQ0FBQztBQUVyRTs7R0FFRztBQUNILE1BQU0sQ0FBQyxNQUFNLHFCQUFxQixHQUFHLHFDQUFxQyxDQUFDO0FBRTNFOztHQUVHO0FBQ0gsTUFBTSxDQUFDLE1BQU0sVUFBVSxHQUFHLGNBQWMsQ0FBQztBQWdEekM7OztHQUdHO0FBQ0gsTUFBTSxlQUFlLEdBQW9CO0lBQ3JDLEtBQUssRUFBRSxLQUFLO0lBQ1osUUFBUSxFQUFFLElBQUk7SUFDZCxNQUFNLEVBQUUsSUFBSTtJQUNaLE9BQU8sRUFBRSxJQUFJO0lBQ2IsSUFBSSxFQUFFLElBQUk7SUFDVixRQUFRLEVBQUUsSUFBSTtJQUNkLFdBQVcsRUFBRSxJQUFJO0lBQ2pCLE1BQU0sRUFBRSxJQUFJO0lBQ1osSUFBSSxFQUFFLElBQUk7Q0FDYixDQUFDO0FBRUY7OztHQUdHO0FBQ0gsTUFBTSxXQUFXLEdBQUcsQ0FBQyxLQUE4QixFQUFpQixFQUFFO0lBQ2xFLElBQUksS0FBSyxLQUFLLElBQUk7UUFBRSxPQUFPLElBQUksQ0FBQztJQUNoQyxPQUFPLEtBQUssQ0FBQyxDQUFDLENBQUMsQ0FBQztBQUNwQixDQUFDLENBQUM7QUFFRixNQUFNLFdBQVcsR0FBRyxPQUFPLENBQUM7QUFFNUI7OztHQUdHO0FBQ0gsTUFBTSxRQUFRLEdB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RyxDQUFDLE9BQWUsRUFBRSxLQUFhLEVBQVUsRUFBRSxDQUN0RCxLQUFLO0tBQ0EsS0FBSyxDQUFDLEVBQUUsQ0FBQztLQUNULE1BQU0sQ0FBVyxDQUFDLEdBQUcsRUFBRSxDQUFDLEVBQUUsQ0FBQyxFQUFFLEVBQUU7SUFDNUIsTUFBTSxNQUFNLEdBQUcsT0FBTyxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsQ0FBQztJQUNqQyxJQUFJLE1BQU0sS0FBSyxHQUFHO1FBQUUsR0FBRyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUM7SUFDL0MsSUFBSSxNQUFNLEtBQUssR0FBRztRQUFFLEdBQUcsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDO0lBQy9DLElBQUksTUFBTSxLQUFLLEdBQUc7UUFBRSxHQUFHLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDO0lBQ2hDLE9BQU8sR0FBRyxDQUFDO0FBQ2YsQ0FBQyxFQUFFLEVBQUUsQ0FBQztLQUNMLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQyIsImZpbGUiOiJjb21wb25lbnRzL3Bvc3Rjb2RlLmpzIiwic291cmNlc0NvbnRlbnQiOlsiLy8gZnJvbSBodHRwczovL2dpdGh1Yi5jb20vaWRlYWwtcG9zdGNvZGVzL3Bvc3Rjb2RlXHJcblxyXG4vKipcclxuICogQGhpZGRlblxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IERJU1RSSUNUX1NQTElUX1JFR0VYID0gL14oW2Etel17MSwyfVxcZCkoW2Etel0pJC9pO1xyXG4vKipcclxuICogVGVzdHMgZm9yIHRoZSB1bml0IHNlY3Rpb24gb2YgYSBwb3N0Y29kZVxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IFVOSVRfUkVHRVggPSAvW2Etel17Mn0kL2k7XHJcbi8qKlxyXG4gKiBUZXN0cyBmb3IgdGhlIGlud2FyZCBjb2RlIHNlY3Rpb24gb2YgYSBwb3N0Y29kZVxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IElOQ09ERV9SRUdFWCA9IC9cXGRbYS16XXsyfSQvaTtcclxuLyoqXHJcbiAqIFRlc3RzIGZvciB0aGUgb3V0d2FyZCBjb2RlIHNlY3Rpb24gb2YgYSBwb3N0Y29kZVxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IE9VVENPREVfUkVHRVggPSAvXlthLXpdezEsMn1cXGRbYS16XFxkXT8kL2k7XHJcbi8qKlxyXG4gKiBUZXN0cyBmb3IgYSB2YWxpZCBwb3N0Y29kZVxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IFBPU1RDT0RFX1JFR0VYID0gL15bYS16XXsxLDJ9XFxkW2EtelxcZF0/XFxzKlxcZFthLXpdezJ9JC9pO1xyXG5cclxuLyoqXHJcbiAqIFRlc3QgZm9yIGEgdmFsaWQgcG9zdGNvZGUgZW1iZWRkZWQgaW4gdGV4dFxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IFBPU1RDT0RFX0NPUlBVU19SRUdFWCA9IC9bYS16XXsxLDJ9XFxkW2EtelxcZF0/XFxzKlxcZFthLXpdezJ9L2dpO1xyXG5cclxuLyoqXHJcbiAqIFRlc3RzIGZvciB0aGUgYXJlYSBzZWN0aW9uIG9mIGEgcG9zdGNvZGVcclxuICovXHJcbmV4cG9ydCBjb25zdCBBUkVBX1JFR0VYID0gL15bYS16XXsxLDJ9L2k7XHJcblxyXG4vKipcclxuICogQGhpZGRlblxyXG4gKi9cclxuaW50ZXJmYWNlIFZhbGlkYXRvciB7XHJcbiAgICAoaW5wdXQ6IHN0cmluZyk6IGJvb2xlYW47XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBAaGlkZGVuXHJcbiAqL1xyXG5pbnRlcmZhY2UgUGFyc2VyIHtcclxuICAgIC8qKlxyXG4gICAgICogQGhpZGRlblxyXG4gICAgICovXHJcbiAgICAocG9zdGNvZGU6IHN0cmluZyk6IHN0cmluZyB8IG51bGw7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBSZXByZXNlbnRzIGEgdmFsaWQgcG9zdGNvZGVcclxuICpcclxuICogTm90ZSB0aGF0IHJlc3VsdHMgd2lsbCBiZSBub3JtYWxpc2VkIChpLmUuIGNvcnJlY3RseSBmb3JtYXR0ZWQpLCBpbmNsdWRpbmcgYHBvc3Rjb2RlYFxyXG4gKi9cclxudHlwZSBWYWxpZFBvc3Rjb2RlID0ge1xyXG4gICAgdmFsaWQ6IHRydWU7XHJcbiAgICBwb3N0Y29kZTogc3RyaW5nO1xyXG4gICAgaW5jb2RlOiBzdHJpbmc7XHJcbiAgICBvdXRjb2RlOiBzdHJpbmc7XHJcbiAgICBhcmVhOiBzdHJpbmc7XHJcbiAgICBkaXN0cmljdDogc3RyaW5nO1xyXG4gICAgc3ViRGlzdHJpY3Q6IHN0cmluZyB8IG51bGw7XHJcbiAgICBzZWN0b3I6IHN0cmluZztcclxuICAgIHVuaXQ6IHN0cmluZztcclxufTtcclxuXHJcbnR5cGUgSW52YWxpZFBvc3Rjb2RlID0ge1xyXG4gICAgdmFsaWQ6IGZhbHNlO1xyXG4gICAgcG9zdGNvZGU6IG51bGw7XHJcbiAgICBpbmNvZGU6IG51bGw7XHJcbiAgICBvdXRjb2RlOiBudWxsO1xyXG4gICAgYXJlYTogbnVsbDtcclxuICAgIGRpc3RyaWN0OiBudWxsO1xyXG4gICAgc3ViRGlzdHJpY3Q6IG51bGw7XHJcbiAgICBzZWN0b3I6IG51bGw7XHJcbiAgICB1bml0OiBudWxsO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEludmFsaWQgcG9zdGNvZGUgcHJvdG90eXBlXHJcbiAqIEBoaWRkZW5cclxuICovXHJcbmNvbnN0IGludmFsaWRQb3N0Y29kZTogSW52YWxpZFBvc3Rjb2RlID0ge1xyXG4gICAgdmFsaWQ6IGZhbHNlLFxyXG4gICAgcG9zdGNvZGU6IG51bGwsXHJcbiAgICBpbmNvZGU6IG51bGwsXHJcbiAgICBvdXRjb2RlOiBudWxsLFxyXG4gICAgYXJlYTogbnVsbCxcclxuICAgIGRpc3RyaWN0OiBudWxsLFxyXG4gICAgc3ViRGlzdHJpY3Q6IG51bGwsXHJcbiAgICBzZWN0b3I6IG51bGwsXHJcbiAgICB1bml0OiBudWxsLFxyXG59O1xyXG5cclxuLyoqXHJcbiAqIFJldHVybiBmaXJzdCBlbGVtIG9mIGlucHV0IGlzIFJlZ0V4cE1hdGNoQXJyYXkgb3IgbnVsbCBpZiBpbnB1dCBudWxsXHJcbiAqIEBoaWRkZW5cclxuICovXHJcbmNvbnN0IGZpcnN0T3JOdWxsID0gKG1hdGNoOiBSZWdFeHBNYXRjaEFycmF5IHwgbnVsbCk6IHN0cmluZyB8IG51bGwgPT4ge1xyXG4gICAgaWYgKG1hdGNoID09PSBudWxsKSByZXR1cm4gbnVsbDtcclxuICAgIHJldHVybiBtYXRjaFswXTtcclxufTtcclxuXHJcbmNvbnN0IFNQQUNFX1JFR0VYID0gL1xccysvZ2k7XHJcblxyXG4vKipcclxuICogRHJvcCBhbGwgc3BhY2VzIGFuZCB1cHBlcmNhc2VcclxuICogQGhpZGRlblxyXG4gKi9cclxuY29uc3Qgc2FuaXRpemUgPSAoczogc3RyaW5nKTogc3RyaW5nID0+XHJcbiAgICBzLnJlcGxhY2UoU1BBQ0VfUkVHRVgsIFwiXCIpLnRvVXBwZXJDYXNlKCk7XHJcblxyXG4vKipcclxuICogU2FuaXRpemVzIHN0cmluZyBhbmQgcmV0dXJucyByZWdleCBtYXRjaGVzXHJcbiAqIEBoaWRkZW5cclxuICovXHJcbmNvbnN0IG1hdGNoT24gPSAoczogc3RyaW5nLCByZWdleDogUmVnRXhwKTogUmVnRXhwTWF0Y2hBcnJheSB8IG51bGwgPT5cclxuICAgIHNhbml0aXplKHMpLm1hdGNoKHJlZ2V4KTtcclxuXHJcbi8qKlxyXG4gKiBEZXRlY3RzIGEgXCJ2YWxpZFwiIHBvc3Rjb2RlXHJcbiAqIC0gU3RhcnRzIGFuZCBlbmRzIG9uIGEgbm9uLXNwYWNlIGNoYXJhY3RlclxyXG4gKiAtIEFueSBsZW5ndGggb2YgaW50ZXJ2ZW5pbmcgc3BhY2UgaXMgYWxsb3dlZFxyXG4gKiAtIE11c3QgY29uZm9ybSB0byBvbmUgb2YgZm9sbG93aW5nIHNjaGVtYXM6XHJcbiAqICAtIEFBMUEgMUFBXHJcbiAqICAtIEExQSAxQUFcclxuICogIC0gQTEgMUFBXHJcbiAqICAtIEE5OSA5QUFcclxuICogIC0gQUE5IDlBQVxyXG4gKiAgLSBBQTk5IDlBQVxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IGlzVmFsaWQ6IFZhbGlkYXRvciA9IChwb3N0Y29kZSkgPT5cclxuICAgIHBvc3Rjb2RlLm1hdGNoKFBPU1RDT0RFX1JFR0VYKSAhPT0gbnVsbDtcclxuXHJcbi8qKlxyXG4gKiBSZXR1cm5zIHRydWUgaWYgc3RyaW5nIGlzIGEgdmFsaWQgb3V0Y29kZVxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IHZhbGlkT3V0Y29kZTogVmFsaWRhdG9yID0gKG91dGNvZGUpID0+XHJcbiAgICBvdXRjb2RlLm1hdGNoKE9VVENPREVfUkVHRVgpICE9PSBudWxsO1xyXG5cclxuLyoqXHJcbiAqIFJldHVybnMgYSBub3JtYWxpc2VkIHBvc3Rjb2RlIHN0cmluZyAoaS5lLiB1cHBlcmNhc2VkIGFuZCBwcm9wZXJseSBzcGFjZWQpXHJcbiAqXHJcbiAqIFJldHVybnMgbnVsbCBpZiBpbnZhbGlkIHBvc3Rjb2RlXHJcbiAqL1xyXG5leHBvcnQgY29uc3QgdG9Ob3JtYWxpc2VkOiBQYXJzZXIgPSAocG9zdGNvZGUpID0+IHtcclxuICAgIGNvbnN0IG91dGNvZGUgPSB0b091dGNvZGUocG9zdGNvZGUpO1xyXG4gICAgaWYgKG91dGNvZGUgPT09IG51bGwpIHJldHVybiBudWxsO1xyXG4gICAgY29uc3QgaW5jb2RlID0gdG9JbmNvZGUocG9zdGNvZGUpO1xyXG4gICAgaWYgKGluY29kZSA9PT0gbnVsbCkgcmV0dXJuIG51bGw7XHJcbiAgICByZXR1cm4gYCR7b3V0Y29kZX0gJHtpbmNvZGV9YDtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZXR1cm5zIGEgY29ycmVjdGx5IGZvcm1hdHRlZCBvdXRjb2RlIGdpdmVuIGEgcG9zdGNvZGVcclxuICpcclxuICogUmV0dXJucyBudWxsIGlmIGludmFsaWQgcG9zdGNvZGVcclxuICovXHJcbmV4cG9ydCBjb25zdCB0b091dGNvZGU6IFBhcnNlciA9IChwb3N0Y29kZSkgPT4ge1xyXG4gICAgaWYgKCFpc1ZhbGlkKHBvc3Rjb2RlKSkgcmV0dXJuIG51bGw7XHJcbiAgICByZXR1cm4gc2FuaXRpemUocG9zdGNvZGUpLnJlcGxhY2UoSU5DT0RFX1JFR0VYLCBcIlwiKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZXR1cm5zIGEgY29ycmVjdGx5IGZvcm1hdHRlZCBpbmNvZGUgZ2l2ZW4gYSBwb3N0Y29kZVxyXG4gKlxyXG4gKiBSZXR1cm5zIG51bGwgaWYgaW52YWxpZCBwb3N0Y29kZVxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IHRvSW5jb2RlOiBQYXJzZXIgPSAocG9zdGNvZGUpID0+IHtcclxuICAgIGlmICghaXNWYWxpZChwb3N0Y29kZSkpIHJldHVybiBudWxsO1xyXG4gICAgY29uc3QgbWF0Y2ggPSBtYXRjaE9uKHBvc3Rjb2RlLCBJTkNPREVfUkVHRVgpO1xyXG4gICAgcmV0dXJuIGZpcnN0T3JOdWxsKG1hdGNoKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZXR1cm5zIGEgY29ycmVjdGx5IGZvcm1hdHRlZCBhcmVhIGdpdmVuIGEgcG9zdGNvZGVcclxuICpcclxuICogUmV0dXJucyBudWxsIGlmIGludmFsaWQgcG9zdGNvZGVcclxuICovXHJcbmV4cG9ydCBjb25zdCB0b0FyZWE6IFBhcnNlciA9IChwb3N0Y29kZSkgPT4ge1xyXG4gICAgaWYgKCFpc1ZhbGlkKHBvc3Rjb2RlKSkgcmV0dXJuIG51bGw7XHJcbiAgICBjb25zdCBtYXRjaCA9IG1hdGNoT24ocG9zdGNvZGUsIEFSRUFfUkVHRVgpO1xyXG4gICAgcmV0dXJuIGZpcnN0T3JOdWxsKG1hdGNoKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZXR1cm5zIGEgY29ycmVjdGx5IGZvcm1hdHRlZCBzZWN0b3IgZ2l2ZW4gYSBwb3N0Y29kZVxyXG4gKlxyXG4gKiBSZXR1cm5zIG51bGwgaWYgaW52YWxpZCBwb3N0Y29kZVxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IHRvU2VjdG9yOiBQYXJzZXIgPSAocG9zdGNvZGUpID0+IHtcclxuICAgIGNvbnN0IG91dGNvZGUgPSB0b091dGNvZGUocG9zdGNvZGUpO1xyXG4gICAgaWYgKG91dGNvZGUgPT09IG51bGwpIHJldHVybiBudWxsO1xyXG4gICAgY29uc3QgaW5jb2RlID0gdG9JbmNvZGUocG9zdGNvZGUpO1xyXG4gICAgaWYgKGluY29kZSA9PT0gbnVsbCkgcmV0dXJuIG51bGw7XHJcbiAgICByZXR1cm4gYCR7b3V0Y29kZX0gJHtpbmNvZGVbMF19YDtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZXR1cm5zIGEgY29ycmVjdGx5IGZvcm1hdHRlZCB1bml0IGdpdmVuIGEgcG9zdGNvZGVcclxuICpcclxuICogUmV0dXJucyBudWxsIGlmIGludmFsaWQgcG9zdGNvZGVcclxuICovXHJcbmV4cG9ydCBjb25zdCB0b1VuaXQ6IFBhcnNlciA9IChwb3N0Y29kZSkgPT4ge1xyXG4gICAgaWYgKCFpc1ZhbGlkKHBvc3Rjb2RlKSkgcmV0dXJuIG51bGw7XHJcbiAgICBjb25zdCBtYXRjaCA9IG1hdGNoT24ocG9zdGNvZGUsIFVOSVRfUkVHRVgpO1xyXG4gICAgcmV0dXJuIGZpcnN0T3JOdWxsKG1hdGNoKTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZXR1cm5zIGEgY29ycmVjdGx5IGZvcm1hdHRlZCBkaXN0cmljdCBnaXZlbiBhIHBvc3Rjb2RlXHJcbiAqXHJcbiAqIFJldHVybnMgbnVsbCBpZiBpbnZhbGlkIHBvc3Rjb2RlXHJcbiAqXHJcbiAqIEBleGFtcGxlXHJcbiAqXHJcbiAqIGBgYFxyXG4gKiB0b0Rpc3RyaWN0KFwiQUE5IDlBQVwiKSAvLyA9PiBcIkFBOVwiXHJcbiAqIHRvRGlzdHJpY3QoXCJBOUEgOUFBXCIpIC8vID0+IFwiQTlcIlxyXG4gKiBgYGBcclxuICovXHJcbmV4cG9ydCBjb25zdCB0b0Rpc3RyaWN0OiBQYXJzZXIgPSAocG9zdGNvZGUpID0+IHtcclxuICAgIGNvbnN0IG91dGNvZGUgPSB0b091dGNvZGUocG9zdGNvZGUpO1xyXG4gICAgaWYgKG91dGNvZGUgPT09IG51bGwpIHJldHVybiBudWxsO1xyXG4gICAgY29uc3QgbWF0Y2ggPSBvdXRjb2RlLm1hdGNoKERJU1RSSUNUX1NQTElUX1JFR0VYKTtcclxuICAgIGlmIChtYXRjaCA9PT0gbnVsbCkgcmV0dXJuIG91dGNvZGU7XHJcbiAgICByZXR1cm4gbWF0Y2hbMV07XHJcbn07XHJcblxyXG4vKipcclxuICogUmV0dXJucyBhIGNvcnJlY3RseSBmb3JtYXR0ZWQgc3ViZGlzdHJpY3QgZ2l2ZW4gYSBwb3N0Y29kZVxyXG4gKlxyXG4gKiBSZXR1cm5zIG51bGwgaWYgbm8gc3ViZGlzdHJpY3QgaXMgYXZhaWxhYmxlIG9uIHZhbGlkIHBvc3Rjb2RlXHJcbiAqIFJldHVybnMgbnVsbCBpZiBpbnZhbGlkIHBvc3Rjb2RlXHJcbiAqXHJcbiAqIEBleGFtcGxlXHJcbiAqXHJcbiAqIGBgYFxyXG4gKiB0b1N1YkRpc3RyaWN0KFwiQUE5QSA5QUFcIikgLy8gPT4gXCJBQTlBXCJcclxuICogdG9TdWJEaXN0cmljdChcIkE5QSA5QUFcIikgLy8gPT4gXCJBOUFcIlxyXG4gKiB0b1N1YkRpc3RyaWN0KFwiQUE5IDlBQVwiKSAvLyA9PiBudWxsXHJcbiAqIHRvU3ViRGlzdHJpY3QoXCJBOSA5QUFcIikgLy8gPT4gbnVsbFxyXG4gKiBgYGBcclxuICovXHJcbmV4cG9ydCBjb25zdCB0b1N1YkRpc3RyaWN0OiBQYXJzZXIgPSAocG9zdGNvZGUpID0+IHtcclxuICAgIGNvbnN0IG91dGNvZGUgPSB0b091dGNvZGUocG9zdGNvZGUpO1xyXG4gICAgaWYgKG91dGNvZGUgPT09IG51bGwpIHJldHVybiBudWxsO1xyXG4gICAgY29uc3Qgc3BsaXQgPSBvdXRjb2RlLm1hdGNoKERJU1RSSUNUX1NQTElUX1JFR0VYKTtcclxuICAgIGlmIChzcGxpdCA9PT0gbnVsbCkgcmV0dXJuIG51bGw7XHJcbiAgICByZXR1cm4gb3V0Y29kZTtcclxufTtcclxuXHJcbi8qKlxyXG4gKiBSZXR1cm5zIGEgVmFsaWRQb3N0Y29kZSBvciBJbnZhbGlkUG9zdGNvZGUgb2JqZWN0IGZyb20gYSBwb3N0Y29kZSBzdHJpbmdcclxuICpcclxuICogQGV4YW1wbGVcclxuICpcclxuICogYGBgXHJcbiAqIGltcG9ydCB7IHBhcnNlIH0gZnJvbSBcInBvc3Rjb2RlXCI7XHJcbiAqXHJcbiAqIGNvbnN0IHtcclxuICogcG9zdGNvZGUsICAgIC8vID0+IFwiU1cxQSAyQUFcIlxyXG4gKiBvdXRjb2RlLCAgICAgLy8gPT4gXCJTVzFBXCJcclxuICogaW5jb2RlLCAgICAgIC8vID0+IFwiMkFBXCJcclxuICogYXJlYSwgICAgICAgIC8vID0+IFwiU1dcIlxyXG4gKiBkaXN0cmljdCwgICAgLy8gPT4gXCJTVzFcIlxyXG4gKiB1bml0LCAgICAgICAgLy8gPT4gXCJBQVwiXHJcbiAqIHNlY3RvciwgICAgICAvLyA9PiBcIlNXMUEgMlwiXHJcbiAqIHN1YkRpc3RyaWN0LCAvLyA9PiBcIlNXMUFcIlxyXG4gKiB2YWxpZCwgICAgICAgLy8gPT4gdHJ1ZVxyXG4gKiB9ID0gcGFyc2UoXCJTdzFBICAgICAyYWFcIik7XHJcbiAqXHJcbiAqIGNvbnN0IHtcclxuICogcG9zdGNvZGUsICAgIC8vID0+IG51bGxcclxuICogb3V0Y29kZSwgICAgIC8vID0+IG51bGxcclxuICogaW5jb2RlLCAgICAgIC8vID0+IG51bGxcclxuICogYXJlYSwgICAgICAgIC8vID0+IG51bGxcclxuICogZGlzdHJpY3QsICAgIC8vID0+IG51bGxcclxuICogdW5pdCwgICAgICAgIC8vID0+IG51bGxcclxuICogc2VjdG9yLCAgICAgIC8vID0+IG51bGxcclxuICogc3ViRGlzdHJpY3QsIC8vID0+IG51bGxcclxuICogdmFsaWQsICAgICAgIC8vID0+IGZhbHNlXHJcbiAqIH0gPSBwYXJzZShcIiAgICBPaCBubywgKTogICBcIik7XHJcbiAqIGBgYFxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IHBhcnNlID0gKHBvc3Rjb2RlOiBzdHJpbmcpOiBWYWxpZFBvc3Rjb2RlIHwgSW52YWxpZFBvc3Rjb2RlID0+IHtcclxuICAgIGlmICghaXNWYWxpZChwb3N0Y29kZSkpIHJldHVybiB7IC4uLmludmFsaWRQb3N0Y29kZSB9O1xyXG4gICAgcmV0dXJuIHtcclxuICAgICAgICB2YWxpZDogdHJ1ZSxcclxuICAgICAgICBwb3N0Y29kZTogdG9Ob3JtYWxpc2VkKHBvc3Rjb2RlKSBhcyBzdHJpbmcsXHJcbiAgICAgICAgaW5jb2RlOiB0b0luY29kZShwb3N0Y29kZSkgYXMgc3RyaW5nLFxyXG4gICAgICAgIG91dGNvZGU6IHRvT3V0Y29kZShwb3N0Y29kZSkgYXMgc3RyaW5nLFxyXG4gICAgICAgIGFyZWE6IHRvQXJlYShwb3N0Y29kZSkgYXMgc3RyaW5nLFxyXG4gICAgICAgIGRpc3RyaWN0OiB0b0Rpc3RyaWN0KHBvc3Rjb2RlKSBhcyBzdHJpbmcsXHJcbiAgICAgICAgc3ViRGlzdHJpY3Q6IHRvU3ViRGlzdHJpY3QocG9zdGNvZGUpLFxyXG4gICAgICAgIHNlY3RvcjogdG9TZWN0b3IocG9zdGNvZGUpIGFzIHN0cmluZyxcclxuICAgICAgICB1bml0OiB0b1VuaXQocG9zdGNvZGUpIGFzIHN0cmluZyxcclxuICAgIH07XHJcbn07XHJcblxyXG4vKipcclxuICogU2VhcmNoZXMgYSBib2R5IG9mIHRleHQgZm9yIHBvc3Rjb2RlIG1hdGNoZXNcclxuICpcclxuICogUmV0dXJucyBhbiBlbXB0eSBhcnJheSBpZiBubyBtYXRjaFxyXG4gKlxyXG4gKiBAZXhhbXBsZVxyXG4gKlxyXG4gKiBgYGBcclxuICogLy8gUmV0cmlldmUgdmFsaWQgcG9zdGNvZGVzIGluIGEgYm9keSBvZiB0ZXh0XHJcbiAqIGNvbnN0IG1hdGNoZXMgPSBtYXRjaChcIlRoZSBQTSBhbmQgaGVyIG5vLjIgbGl2ZSBhdCBTVzFBMmFhIGFuZCBTVzFBIDJBQlwiKTsgLy8gPT4gW1wiU1cxQTJhYVwiLCBcIlNXMUEgMkFCXCJdXHJcbiAqXHJcbiAqIC8vIFBlcmZvcm0gdHJhbnNmb3JtYXRpb25zIGxpa2Ugbm9ybWFsaXNhdGlvbiBwb3N0Y29kZXMgdXNpbmcgYC5tYXBgIGFuZCBgdG9Ob3JtYWxpc2VkYFxyXG4gKiBtYXRjaGVzLm1hcCh0b05vcm1hbGlzZWQpOyAvLyA9PiBbXCJTVzFBIDJBQVwiLCBcIlNXMUEgMkFCXCJdXHJcbiAqXHJcbiAqIC8vIE5vIG1hdGNoZXMgeWllbGRzIGVtcHR5IGFycmF5XHJcbiAqIG1hdGNoKFwiU29tZSBMb25kb24gb3V0d2FyZCBjb2RlcyBhcmUgU1cxQSwgTlcxIGFuZCBFMVwiKTsgLy8gPT4gW11cclxuICogYGBgXHJcbiAqL1xyXG5leHBvcnQgY29uc3QgbWF0Y2ggPSAoY29ycHVzOiBzdHJpbmcpOiBzdHJpbmdbXSA9PlxyXG4gICAgY29ycHVzLm1hdGNoKFBPU1RDT0RFX0NPUlBVU19SRUdFWCkgfHwgW107XHJcblxyXG4vKipcclxuICogQGhpZGRlblxyXG4gKi9cclxuaW50ZXJmYWNlIFJlcGxhY2VSZXN1bHQge1xyXG4gICAgLyoqXHJcbiAgICAgKiBMaXN0IG9mIG1hdGNoaW5nIHBvc3Rjb2RlcyBmb3VuZCBpbnRleHRcclxuICAgICAqL1xyXG4gICAgbWF0Y2g6IHN0cmluZ1tdO1xyXG4gICAgLyoqXHJcbiAgICAgKiBCb2R5IG9mIHRleHQgd2l0aCBwb3N0Y29kZXMgcmVwbGFjZWQgKHdpdGggZW1wdHkgc3RyaW5nIGJ5IGRlZmF1bHQpXHJcbiAgICAgKi9cclxuICAgIHJlc3VsdDogc3RyaW5nO1xyXG59XHJcblxyXG4vKipcclxuICogUmVwbGFjZXMgcG9zdGNvZGVzIGluIGEgYm9keSBvZiB0ZXh0IHdpdGggYSBzdHJpbmdcclxuICpcclxuICogQnkgZGVmYXVsdCB0aGUgcmVwbGFjZW1lbnQgc3RyaW5nIGlzIGVtcHR5IHN0cmluZyBgXCJcImBcclxuICpcclxuICogQGV4YW1wbGVcclxuICpcclxuICogYGBgXHJcbiAqIC8vIFJlcGxhY2UgcG9zdGNvZGVzIGluIGEgYm9keSBvZiB0ZXh0XHJcbiAqIHJlcGxhY2UoXCJUaGUgUE0gYW5kIGhlciBuby4yIGxpdmUgYXQgU1cxQTJBQSBhbmQgU1cxQSAyQUJcIik7XHJcbiAqIC8vID0+IHsgbWF0Y2g6IFtcIlNXMUEyQUFcIiwgXCJTVzFBIDJBQlwiXSwgcmVzdWx0OiBcIlRoZSBQTSBhbmQgaGVyIG5vLjIgbGl2ZSBhdCAgYW5kIFwiIH1cclxuICpcclxuICogLy8gQWRkIGN1c3RvbSByZXBsYWNlbWVudFxyXG4gKiByZXBsYWNlKFwiVGhlIFBNIGxpdmVzIGF0IFNXMUEgMkFBXCIsIFwiRG93bmluZyBTdHJlZXRcIik7XHJcbiAqIC8vID0+IHsgbWF0Y2g6IFtcIlNXMUEgMkFBXCJdLCByZXN1bHQ6IFwiVGhlIFBNIGxpdmVzIGF0IERvd25pbmcgU3RyZWV0XCIgfTtcclxuICpcclxuICogLy8gTm8gbWF0Y2hcclxuICogcmVwbGFjZShcIlNvbWUgTG9uZG9uIG91dHdhcmQgY29kZXMgYXJlIFNXMUEsIE5XMSBhbmQgRTFcIik7XHJcbiAqIC8vID0+IHsgbWF0Y2g6IFtdLCByZXN1bHQ6IFwiU29tZSBMb25kb24gb3V0d2FyZCBjb2RlcyBhcmUgU1cxQSwgTlcxIGFuZCBFMVwiIH1cclxuICogYGBgXHJcbiAqL1xyXG5leHBvcnQgY29uc3QgcmVwbGFjZSA9IChjb3JwdXM6IHN0cmluZywgcmVwbGFjZVdpdGggPSBcIlwiKTogUmVwbGFjZVJlc3VsdCA9PiAoe1xyXG4gICAgbWF0Y2g6IG1hdGNoKGNvcnB1cyksXHJcbiAgICByZXN1bHQ6IGNvcnB1cy5yZXBsYWNlKFBPU1RDT0RFX0NPUlBVU19SRUdFWCwgcmVwbGFjZVdpdGgpLFxyXG59KTtcclxuXHJcbmV4cG9ydCBjb25zdCBGSVhBQkxFX1JFR0VYID0gL15cXHMqW2EtejAxXXsxLDJ9WzAtOW9pXVthLXpcXGRdP1xccypbMC05b2ldW2EtejAxXXsyfVxccyokL2k7XHJcblxyXG4vKipcclxuICogQXR0ZW1wdHMgdG8gZml4IGFuZCBjbGVhbiBhIHBvc3Rjb2RlLiBTcGVjaWZpY2FsbHk6XHJcbiAqIC0gUGVyZm9ybXMgY2hhcmFjdGVyIGNvbnZlcnNpb24gb24gb2J2aW91c2x5IHdyb25nIGFuZCBjb21tb25seSBtaXhlZCB1cCBsZXR0ZXJzIChlLmcuIE8gPT4gMCBhbmQgdmljZSB2ZXJzYSlcclxuICogLSBUcmltcyBzdHJpbmdcclxuICogLSBQcm9wZXJseSBhZGRzIHNwYWNlIGJldHdlZW4gb3V0d2FyZCBhbmQgaW53YXJkIGNvZGVzXHJcbiAqXHJcbiAqIElmIHRoZSBwb3N0Y29kZSBjYW5ub3QgYmUgY29lcmNlZCBpbnRvIGEgdmFsaWQgZm9ybWF0LCB0aGUgb3JpZ2luYWwgc3RyaW5nIGlzIHJldHVybmVkXHJcbiAqXHJcbiAqIEBleGFtcGxlXHJcbiAqIGBgYGphdmFzY3JpcHRcclxuICogZml4KFwiIFNXMUEgIDJBT1wiKSA9PiBcIlNXMUEgMkFPXCIgLy8gUHJvcGVybHkgc3BhY2VzXHJcbiAqIGZpeChcIlNXMUEgMkEwXCIpID0+IFwiU1cxQSAyQU9cIiAvLyAwIGlzIGNvZXJjZWQgaW50byBcIjBcIlxyXG4gKiBgYGBcclxuICpcclxuICogQWltcyB0byBiZSB1c2VkIGluIGNvbmp1bmN0aW9uIHdpdGggcGFyc2UgdG8gbWFrZSBwb3N0Y29kZSBlbnRyeSBtb3JlIGZvcmdpdmluZzpcclxuICpcclxuICogQGV4YW1wbGVcclxuICogYGBgamF2YXNjcmlwdFxyXG4gKiBjb25zdCB7IGlud2FyZCB9ID0gcGFyc2UoZml4KFwiU1cxQSAyQTBcIikpOyAvLyBpbndhcmQgPSBcIjJBT1wiXHJcbiAqIGBgYFxyXG4gKi9cclxuZXhwb3J0IGNvbnN0IGZpeCA9IChzOiBzdHJpbmcpOiBzdHJpbmcgPT4ge1xyXG4gICAgY29uc3QgbWF0Y2ggPSBzLm1hdGNoKEZJWEFCTEVfUkVHRVgpO1xyXG4gICAgaWYgKG1hdGNoID09PSBudWxsKSByZXR1cm4gcztcclxuICAgIHMgPSBzLnRvVXBwZXJDYXNlKCkudHJpbSgpLnJlcGxhY2UoL1xccysvZ2ksIFwiXCIpO1xyXG4gICAgY29uc3QgbCA9IHMubGVuZ3RoO1xyXG4gICAgY29uc3QgaW53YXJkID0gcy5zbGljZShsIC0gMywgbCk7XHJcbiAgICByZXR1cm4gYCR7Y29lcmNlT3V0Y29kZShzLnNsaWNlKDAsIGwgLSAzKSl9ICR7Y29lcmNlKFwiTkxMXCIsIGlud2FyZCl9YDtcclxufTtcclxuXHJcbmNvbnN0IHRvTGV0dGVyOiBSZWNvcmQ8c3RyaW5nLCBzdHJpbmc+ID0ge1xyXG4gICAgXCIwXCI6IFwiT1wiLFxyXG4gICAgXCIxXCI6IFwiSVwiLFxyXG59O1xyXG5cclxuY29uc3QgdG9OdW1iZXI6IFJlY29yZDxzdHJpbmcsIHN0cmluZz4gPSB7XHJcbiAgICBPOiBcIjBcIixcclxuICAgIEk6IFwiMVwiLFxyXG59O1xyXG5cclxuY29uc3QgY29lcmNlT3V0Y29kZSA9IChpOiBzdHJpbmcpOiBzdHJpbmcgPT4ge1xyXG4gICAgaWYgKGkubGVuZ3RoID09PSAyKSByZXR1cm4gY29lcmNlKFwiTE5cIiwgaSk7XHJcbiAgICBpZiAoaS5sZW5ndGggPT09IDMpIHJldHVybiBjb2VyY2UoXCJMPz9cIiwgaSk7XHJcbiAgICBpZiAoaS5sZW5ndGggPT09IDQpIHJldHVybiBjb2VyY2UoXCJMTE4/XCIsIGkpO1xyXG4gICAgcmV0dXJuIGk7XHJcbn07XHJcblxyXG4vKipcclxuICogR2l2ZW4gYSBwYXR0ZXJuIG9mIGxldHRlcnMsIG51bWJlcnMgYW5kIHVua25vd25zIHJlcHJlc2VudGVkIGFzIGEgc2VxdWVuY2VcclxuICogb2YgTCwgTnMgYW5kID8gcmVzcGVjdGl2ZWx5OyBjb2VyY2UgdGhlbSBpbnRvIHRoZSBjb3JyZWN0IHR5cGUgZ2l2ZW4gYVxyXG4gKiBtYXBwaW5nIG9mIHBvdGVudGlhbGx5IGNvbmZ1c2VkIGxldHRlcnNcclxuICpcclxuICogQGhpZGRlblxyXG4gKlxyXG4gKiBAZXhhbXBsZSBjb2VyY2UoXCJMTE5cIiwgXCIwTzhcIikgPT4gXCJPTzhcIlxyXG4gKi9cclxuY29uc3QgY29lcmNlID0gKHBhdHRlcm46IHN0cmluZywgaW5wdXQ6IHN0cmluZyk6IHN0cmluZyA9PlxyXG4gICAgaW5wdXRcclxuICAgICAgICAuc3BsaXQoXCJcIilcclxuICAgICAgICAucmVkdWNlPHN0cmluZ1tdPigoYWNjLCBjLCBpKSA9PiB7XHJcbiAgICAgICAgICAgIGNvbnN0IHRhcmdldCA9IHBhdHRlcm4uY2hhckF0KGkpO1xyXG4gICAgICAgICAgICBpZiAodGFyZ2V0ID09PSBcIk5cIikgYWNjLnB1c2godG9OdW1iZXJbY10gfHwgYyk7XHJcbiAgICAgICAgICAgIGlmICh0YXJnZXQgPT09IFwiTFwiKSBhY2MucHVzaCh0b0xldHRlcltjXSB8fCBjKTtcclxuICAgICAgICAgICAgaWYgKHRhcmdldCA9PT0gXCI/XCIpIGFjYy5wdXNoKGMpO1xyXG4gICAgICAgICAgICByZXR1cm4gYWNjO1xyXG4gICAgICAgIH0sIFtdKVxyXG4gICAgICAgIC5qb2luKFwiXCIpOyJdfQ==
