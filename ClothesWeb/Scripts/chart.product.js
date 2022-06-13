



function formatPrice(price) {
    if (price.toString().length > 3) {
        return price + ",00 VND";
    }
    if (price === 0) {
        return price + " VNĐ";
    }
    return price + ",000 VNĐ";
}

window.onload = async () => {
    const idProduct = $("#idProduct").val();
    const response = await fetch(`/Bills/ChartProductDetail/${idProduct}`);
    const data = await response.json();
    const tempTotal = data.reduce((a, b) => a + b.totalMoney, 0);
    if (tempTotal) {
        $(".featuredMoney").html(formatPrice(tempTotal));
    }
    const ctx = document.getElementById('productChart').getContext('2d');
    const productChart = new Chart(ctx, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Transaction',
                data: data,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            parsing: {
                xAxisKey: 'Date',
                yAxisKey: 'totalMoney'
            }
        }
    });
}




