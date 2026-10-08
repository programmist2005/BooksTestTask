-- INSERT
CREATE OR REPLACE FUNCTION usp_books_insert(
    p_title           VARCHAR(500),
    p_author          VARCHAR(300),
    p_year_published  INTEGER,
    p_toc_xml         TEXT DEFAULT NULL
)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    new_id INTEGER;
BEGIN
    INSERT INTO Books (Title, Author, YearPublished, TocXml)
    VALUES (
        p_title,
        p_author,
        p_year_published,
        p_toc_xml::XML
    )
    RETURNING Id INTO new_id;

    RETURN new_id;
END;
$$;


-- UPDATE
CREATE OR REPLACE FUNCTION usp_books_update(
    p_id              INTEGER,
    p_title           VARCHAR(500),
    p_author          VARCHAR(300),
    p_year_published  INTEGER,
    p_toc_xml         TEXT DEFAULT NULL
)
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE Books
    SET Title         = p_title,
        Author        = p_author,
        YearPublished = p_year_published,
        TocXml        = p_toc_xml::XML,
        UpdatedAt     = CURRENT_TIMESTAMP
    WHERE Id = p_id;
END;
$$;


-- DELETE
CREATE OR REPLACE FUNCTION usp_books_delete(
    p_id INTEGER
)
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM Books
    WHERE Id = p_id;
END;
$$;


-- SELECT by Id
CREATE OR REPLACE FUNCTION usp_books_get_by_id(
    p_id INTEGER
)
RETURNS TABLE (
    Id              INTEGER,
    Title           VARCHAR(500),
    Author          VARCHAR(300),
    YearPublished   INTEGER,
    TocXml          XML,
    CreatedAt       TIMESTAMP,
    UpdatedAt       TIMESTAMP
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        b.Id,
        b.Title,
        b.Author,
        b.YearPublished,
        b.TocXml,
        b.CreatedAt,
        b.UpdatedAt
    FROM Books b
    WHERE b.Id = p_id;
END;
$$;


-- SELECT - SEARCH (по названию, автору, оглавлению)
CREATE OR REPLACE FUNCTION usp_books_search(
    p_title     VARCHAR(500) DEFAULT NULL,
    p_author    VARCHAR(300) DEFAULT NULL,
    p_toc_text  TEXT DEFAULT NULL
)
RETURNS TABLE (
    Id              INTEGER,
    Title           VARCHAR(500),
    Author          VARCHAR(300),
    YearPublished   INTEGER,
    TocXml          XML,
    CreatedAt       TIMESTAMP,
    UpdatedAt       TIMESTAMP
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        b.Id,
        b.Title,
        b.Author,
        b.YearPublished,
        b.TocXml,
        b.CreatedAt,
        b.UpdatedAt
    FROM Books b
    WHERE (p_title IS NULL OR b.Title ILIKE '%' || p_title || '%')
      AND (p_author IS NULL OR b.Author ILIKE '%' || p_author || '%')
      AND (p_toc_text IS NULL OR b.TocXml::TEXT ILIKE '%' || p_toc_text || '%')
    ORDER BY b.Title;
END;
$$;