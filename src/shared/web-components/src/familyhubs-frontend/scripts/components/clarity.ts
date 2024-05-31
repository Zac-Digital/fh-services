declare global {
    interface Window {
        clarity: any;
    }
}

export default function initClarity(clarityId: string) {
    // if the environment doesn't have a measurement id, don't load analytics
    if (!Boolean(clarityId)) {
        return;
    }

    window.clarity = window.clarity || function() {
        (window.clarity.q = window.clarity.q || []).push(arguments)
    }

    loadClarityScript(clarityId);
}

function loadClarityScript(clarityId) {
    const f = document.getElementsByTagName('script')[0];
    const j = document.createElement('script');
    j.async = true;
    j.src = 'https://www.clarity.ms/tag/' + clarityId;
    f.parentNode.insertBefore(j, f);
}
