export const getRandomEmail = () => {
    let text = "";
    let possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (let i = 0; i < 5; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text + "@" + text + '.com';
}

export const getRandomFullName = () => {
    let firstName = "";
    let surname = "";
    let possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (let i = 0; i < 5; i++)
        firstName += possible.charAt(Math.floor(Math.random() * possible.length));

    for (let i = 0; i < 5; i++)
        surname += possible.charAt(Math.floor(Math.random() * possible.length));

    return firstName + " " + surname;
}
const generateRandomText = (length = 3, possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789") => {
    let text = "";
    for (let i = 0; i < length; i++) {
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
};




const getFormattedDate = () => {
    return new Date().toLocaleDateString('en-GB', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    });
};




export const getRandomServiceName = () => {
    const text = generateRandomText();
    const date = getFormattedDate();
    return `Automated Test LA Service ${text} ${date}`;
};




export const getRandomVCFServiceName = () => {
    const text = generateRandomText();
    const date = getFormattedDate();
    return `Automated Test VCS Service ${text} ${date}`;
};
