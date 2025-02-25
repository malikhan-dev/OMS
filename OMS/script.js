import http from "k6/http"
import { sleep } from "k6"

export const options = {
    vus: 40, // A string specifying the total duration of the test run.
    duration: "60s",
}

const payload = JSON.stringify({
    items: [
        {
            productId: 1,
            count: 200,
            unitPrice: 3000
        }

    ],
    description: 'test'
});

export default function () {
    http.post("https://localhost:7030/Order", payload,
        {
            headers: {
                'Content-Type': 'application/json',
            },
        })
    sleep(1)
}