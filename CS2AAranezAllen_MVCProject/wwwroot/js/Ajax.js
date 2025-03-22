const AjaxGET = async (url) => {
    return await $.ajax({
        url: url,
        type: 'GET',
    });
}

const AjaxPOST = async (url, data) => {
    return await $.ajax({
        url: url,
        type: 'POST',
        data: data,
    });
}