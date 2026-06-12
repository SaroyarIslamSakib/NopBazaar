# Product Requirements Document
## Plugin: Product FAQ Manager
**Platform:** nopCommerce    

---

## 1. Overview

Product FAQ Manager is a nopCommerce plugin that allows store administrators to create and manage Frequently Asked Questions for products. FAQ groups are assigned to products and rendered as an accordion on the product detail page. The plugin also injects structured JSON-LD markup on the product page to enable Google's FAQ rich results in search.

---

## 2. Goals

- Give store owners a way to reduce repetitive customer support queries by surfacing answers directly on the product page.
- Improve organic search visibility through FAQPage structured data (JSON-LD).

---

## 3. Entities

### 3.1 FaqGroup

Represents a collection of FAQ items assigned to one product.

### 3.2 FaqItem

Represents a single Q&A pair belonging to a FaqGroup.

---

## 4. Admin Features

### 4.1 FAQ Groups List Page

**Route:** `/Admin/FaqGroup/List`

Displays all FAQ groups in a DataTables grid.

Columns:
- Name
- Product name (linked)
- Published (yes/no)
- Display Order
- Actions: Edit, Delete

Toolbar:
- "Add new" button → navigates to Create page

### 4.2 Create / Edit FAQ Group

**Route:** `/Admin/FaqGroup/Create` and `/Admin/FaqGroup/Edit/{id}`

Fields:
- Name — text input, localized tab support
- Product — searchable dropdown (maps to existing `Product` entity)
- Published — checkbox
- Display Order — number input

On the Edit page only, below the group fields:
- **FAQ Items panel** — embedded DataTables grid showing all items for this group

FAQ Items grid columns:
- Question (truncated)
- Published
- Display Order
- Actions: Edit (inline or modal), Delete

Add new item button inside this panel opens the FAQ Item form.

### 4.3 FAQ Item Form

Accessible from within the Edit FAQ Group page.

Fields:
- Question — text input, localized
- Answer — rich text editor (nop's standard HTML editor), localized
- Published — checkbox
- Display Order — number input

### 4.4 Delete Behavior

- Deleting a FAQ Group deletes all its FAQ Items (handled at service layer, not by cascading, so interns implement it explicitly).
- Deleting a FAQ Item removes only that item.
- On delete of either, relevant cache keys are evicted.

---

## 5. Storefront Features

### 5.1 FAQ Accordion on Product Page

Behavior:
- Checks if the current product has a published FAQ Group with at least one published FAQ Item.
- If yes, renders an accordion section titled "Frequently Asked Questions" below the product overview.
- Each published FAQ Item renders as an accordion row — question as the header, answer expands on click.
- Items are ordered by `DisplayOrder` ascending.
- If no published group or no published items exist for the product, the widget renders nothing (no empty section).

### 5.2 JSON-LD Structured Data

Injected into the product page.

Uses the `FAQPage` schema type as defined by schema.org and supported by Google Search.

Structure:
```json
{
  "@context": "https://schema.org",
  "@type": "FAQPage",
  "mainEntity": [
    {
      "@type": "Question",
      "name": "Question text here",
      "acceptedAnswer": {
        "@type": "Answer",
        "text": "Answer text here"
      }
    }
  ]
}
```

Rules:
- Only published items from a published group are included.
- HTML tags are stripped from the Answer text before injecting into JSON-LD (plain text only per Google's guidelines).
- If no published FAQs exist for the product, the JSON-LD block is not injected.

---

## 6. Caching

- Cache is populated on first load of the product page.
- Cache is evicted when:
  - A FAQ Group is created, updated, or deleted.
  - A FAQ Item is created, updated, or deleted.
- Use nopCommerce's `IStaticCacheManager` with a defined `CacheKey` object in a dedicated `FaqCacheDefaults` class.

---

## 7. Localization

- `FaqGroup.Name`, `FaqItem.Question`, and `FaqItem.Answer` all implement `ILocalizedEntity`.
- Admin edit pages include the standard nopCommerce localized tabs panel for each localized field.
- Storefront renders the value in the active store language, falling back to the default language if a translation is missing.

---

## 8. Plugin Settings

Accessible via **Configuration → Plugins → FAQ Manager → Configure**

| Setting | Type | Default | Description |
|---|---|---|---|
| ShowFaqCount | bool | false | Show the number of FAQ items next to the section heading |

---

## 9. File & Folder Structure

**Use standard folder structure**

---

## 10. Acceptance Criteria

- Admin can create a FAQ Group and assign it to any product.
- Admin can add, edit, reorder, and delete FAQ Items within a group.
- All text fields support multiple languages via localized tabs.
- Published FAQ accordion appears on the correct product page on the storefront.
- Unpublished groups or items do not appear on the storefront.
- Product page contains valid FAQPage JSON-LD when published FAQs exist.
- JSON-LD is absent when no published FAQs exist for the product.
- Cache is evicted correctly after any create, update, or delete operation.
- Plugin installs and uninstalls cleanly without errors.
