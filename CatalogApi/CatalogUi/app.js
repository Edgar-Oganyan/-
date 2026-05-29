const apiUrl = "/api/products";

async function loadProducts() {
    try {
        const response = await fetch(apiUrl);
        if (!response.ok) throw new Error(`HTTP ${response.status}`);
        const products = await response.json();
        const tbody = document.getElementById("productsBody");
        if (products.length === 0) {
            tbody.innerHTML = '<tr><td colspan="6">Нет товаров. Добавьте первый.</td></tr>';
            return;
        }
        tbody.innerHTML = "";
        products.forEach(p => {
            const row = tbody.insertRow();
            row.insertCell(0).innerText = p.name;
            row.insertCell(1).innerText = p.price + " ₽";
            row.insertCell(2).innerText = p.category;
            row.insertCell(3).innerText = p.stockCount;
            row.insertCell(4).innerText = p.availability === "InStock" ? "В наличии" : "Нет";
            const actions = row.insertCell(5);
            actions.innerHTML = `<button onclick="deleteProduct('${p.id}')">Удалить</button>
                                  <button onclick="changeStock('${p.id}')">Изменить остаток</button>`;
        });
    } catch (error) {
        document.getElementById("productsBody").innerHTML = `<tr><td colspan="6">Ошибка загрузки: ${error.message}. Проверьте, что API запущен на порту 5204 и страница открыта по адресу http://localhost:5204/index.html</td></tr>`;
        console.error(error);
    }
}

async function createProduct() {
    const name = document.getElementById("name").value.trim();
    const price = parseFloat(document.getElementById("price").value);
    const category = document.getElementById("category").value.trim();
    const description = document.getElementById("description").value.trim();
    const stockCount = parseInt(document.getElementById("stockCount").value);
    if (!name || isNaN(price) || !category || isNaN(stockCount)) {
        alert("Заполните все поля корректно");
        return;
    }
    const newProduct = { name, price, category, description, stockCount };
    const response = await fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newProduct)
    });
    if (response.ok) {
        document.getElementById("name").value = "";
        document.getElementById("price").value = "";
        document.getElementById("category").value = "";
        document.getElementById("description").value = "";
        document.getElementById("stockCount").value = "";
        await loadProducts();
    } else {
        alert("Ошибка добавления");
    }
}

async function deleteProduct(id) {
    if (confirm("Удалить товар?")) {
        await fetch(`${apiUrl}/${id}`, { method: "DELETE" });
        await loadProducts();
    }
}

async function changeStock(id) {
    const newCount = prompt("Введите новое количество на складе (целое число):");
    if (newCount !== null) {
        const count = parseInt(newCount);
        if (!isNaN(count)) {
            await fetch(`${apiUrl}/${id}/stock`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(count)
            });
            await loadProducts();
        }
    }
}

document.getElementById("addBtn").onclick = createProduct;
loadProducts();