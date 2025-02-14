const products = JSON.parse(localStorage.getItem("products")) || []
const categories = ["Electronics", "Clothing", "Home & Garden", "Books", "Toys"]

function saveProducts() {
  localStorage.setItem("products", JSON.stringify(products))
}

function renderView(view) {
  const viewContent = document.getElementById("view-content")
  const viewTitle = document.getElementById("view-title")
  viewContent.innerHTML = ""
  viewContent.classList.add("fade-in")

  switch (view) {
    case "home":
      viewTitle.textContent = "Dashboard Overview"
      renderHomePage()
      break
    case "list":
      viewTitle.textContent = "Product List"
      renderProductList()
      break
    case "add":
      viewTitle.textContent = "Add New Product"
      renderProductForm()
      break
    case "edit":
      viewTitle.textContent = "Edit Product"
      // The edit form will be rendered when an edit button is clicked
      break
  }

  // Update active nav item
  document.querySelectorAll(".nav-item").forEach((item) => {
    item.classList.remove("active")
    if (item.dataset.view === view) {
      item.classList.add("active")
    }
  })
}

function renderHomePage() {
  const viewContent = document.getElementById("view-content")
  const currentProducts = getCurrentProducts()

  const totalProducts = currentProducts.length
  const totalValue = currentProducts.reduce((sum, product) => sum + product.price, 0)
  const averagePrice = totalProducts > 0 ? totalValue / totalProducts : 0

  viewContent.innerHTML = `
        <div class="row">
            <div class="col-md-4 mb-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Total Products</h5>
                        <p class="card-text display-4">${totalProducts}</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Total Value</h5>
                        <p class="card-text display-4">$${totalValue.toFixed(2)}</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Average Price</h5>
                        <p class="card-text display-4">$${averagePrice.toFixed(2)}</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 mb-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Products by Category</h5>
                        <div class="chart-container">
                            <canvas id="categoryChart"></canvas>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Recent Products</h5>
                        <ul class="list-group list-group-flush">
                            ${currentProducts
                              .slice(0, 5)
                              .map(
                                (product) => `
                                <li class="list-group-item d-flex justify-content-between align-items-center">
                                    ${product.name}
                                    <span class="badge bg-primary rounded-pill">$${product.price.toFixed(2)}</span>
                                </li>
                            `,
                              )
                              .join("")}
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    `

  // Create the category chart
  const categoryData = categories.map(
    (category) => currentProducts.filter((product) => product.category === category).length,
  )

  new Chart(document.getElementById("categoryChart"), {
    type: "doughnut",
    data: {
      labels: categories,
      datasets: [
        {
          data: categoryData,
          backgroundColor: ["#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF"],
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
    },
  })
}

function renderProductList() {
  const viewContent = document.getElementById("view-content")
  const table = document.createElement("table")
  table.id = "productTable"
  table.className = "table table-striped"
  viewContent.appendChild(table)

  const currentProducts = getCurrentProducts()

  $(document).ready(() => {
    $("#productTable").DataTable({
      data: currentProducts,
      columns: [
        {
          title: "Image",
          data: "image",
          render: (data) => `<img src="${data}" class="product-image" alt="Product Image">`,
        },
        { title: "Name", data: "name" },
        { title: "Category", data: "category" },
        {
          title: "Price",
          data: "price",
          render: (data) => `$${data.toFixed(2)}`,
        },
        { title: "Start Date", data: "startDate" },
        { title: "Duration (days)", data: "duration" },
        {
          title: "Actions",
          data: null,
          render: (data, type, row, meta) => `
                            <button class="btn btn-sm btn-warning me-2" onclick="editProduct(${meta.row})">
                                <i class="fas fa-edit"></i> Edit
                            </button>
                            <button class="btn btn-sm btn-danger" onclick="deleteProduct(${meta.row})">
                                <i class="fas fa-trash"></i> Delete
                            </button>
                        `,
        },
      ],
    })
  })
}

function renderProductForm(product = null) {
  const viewContent = document.getElementById("view-content")
  const form = document.createElement("form")
  form.id = "productForm"
  form.innerHTML = `
        <div class="mb-3">
            <label for="name" class="form-label">Name</label>
            <input type="text" class="form-control" id="name" required>
        </div>
        <div class="mb-3">
            <label for="category" class="form-label">Category</label>
            <select class="form-control" id="category" required>
                ${categories.map((category) => `<option value="${category}">${category}</option>`).join("")}
            </select>
        </div>
        <div class="mb-3">
            <label for="price" class="form-label">Price</label>
            <input type="number" class="form-control" id="price" step="0.01" required>
        </div>
        <div class="mb-3">
            <label for="startDate" class="form-label">Start Date</label>
            <input type="date" class="form-control" id="startDate" required>
        </div>
        <div class="mb-3">
            <label for="duration" class="form-label">Duration (days)</label>
            <input type="number" class="form-control" id="duration" required>
        </div>
        <div class="mb-3">
            <label for="image" class="form-label">Product Image</label>
            <input type="file" class="form-control" id="image" accept=".jpg,.jpeg,.png" required>
            <small class="form-text text-muted">Max file size: 1MB. Allowed formats: JPG, JPEG, PNG</small>
        </div>
        <button type="submit" class="btn btn-primary">
            <i class="fas fa-save"></i> ${product ? "Update" : "Add"} Product
        </button>
    `
  viewContent.appendChild(form)

  if (product) {
    document.getElementById("name").value = product.name
    document.getElementById("category").value = product.category
    document.getElementById("price").value = product.price
    document.getElementById("startDate").value = product.startDate
    document.getElementById("duration").value = product.duration
  }

  form.addEventListener("submit", function (e) {
    e.preventDefault()
    const formData = new FormData(this)
    const imageFile = formData.get("image")

    if (imageFile.size > 1024 * 1024) {
      alert("Image size must be less than 1MB")
      return
    }

    const reader = new FileReader()
    reader.onload = (event) => {
      const productData = {
        name: formData.get("name"),
        category: formData.get("category"),
        price: Number.parseFloat(formData.get("price")),
        startDate: formData.get("startDate"),
        duration: Number.parseInt(formData.get("duration")),
        creationDate: new Date().toISOString(),
        createdBy: "currentUserId", // Replace with actual user ID when authentication is implemented
        image: event.target.result,
      }

      if (product) {
        updateProduct(products.indexOf(product), productData)
      } else {
        addProduct(productData)
      }

      renderView("list")
    }
    reader.readAsDataURL(imageFile)
  })
}

function addProduct(product) {
  products.push(product)
  saveProducts()
}

function editProduct(index) {
  renderView("edit")
  renderProductForm(products[index])
}

function updateProduct(index, updatedProduct) {
  products[index] = { ...products[index], ...updatedProduct }
  saveProducts()
}

function deleteProduct(index) {
  if (confirm("Are you sure you want to delete this product?")) {
    products.splice(index, 1)
    saveProducts()
    renderView("list")
  }
}

function getCurrentProducts() {
  const currentDate = new Date()
  return products.filter((product) => {
    const startDate = new Date(product.startDate)
    const endDate = new Date(startDate.getTime() + product.duration * 24 * 60 * 60 * 1000)
    return currentDate >= startDate && currentDate <= endDate
  })
}

// Event listeners for navigation
document.querySelectorAll(".nav-item").forEach((item) => {
  item.addEventListener("click", () => {
    renderView(item.dataset.view)
  })
})

// Initial render
renderView("home")

