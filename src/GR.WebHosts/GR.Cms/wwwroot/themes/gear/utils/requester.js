const customAjaxRequest = async (requestUrl, requestType, requestData = {}, contentTypeJson = false) => {
	return new Promise((resolve, reject) => {
		$.ajax({
			url: requestUrl,
			type: requestType,
			contentType: contentTypeJson ? "application/json" : "application/x-www-form-urlencoded",
			data: requestData,
			success: data => {
				if (Array.isArray(data)) {
					resolve(data);
				}
				else {
					if (data.is_success) {
						resolve(data.result);
					} else if (!data.is_success) {
						reject(data.error_keys);
					} else {
						resolve(data);
					}
				}
			},
			error: e => {
				reject(e);
			}
		});
	});
}
