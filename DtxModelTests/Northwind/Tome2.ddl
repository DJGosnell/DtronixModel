<?xml version="1.0"?>
<Database xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="TomeTest">
  <Table Name="Volumes" Member="Volumes">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="number" Member="number" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="description" Member="description" Type="System.String" DbType="TEXT" IsPrimaryKey="false" Nullable="false" />
    <Column Name="Manga_id" Member="Manga_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="Manga" Member="Manga">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="status_enum" Member="status_enum" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="last_updated" Member="last_updated" Type="System.DateTime" DbType="DATETIME" IsPrimaryKey="false" Nullable="false" />
    <Column Name="total_volumes_released" Member="total_volumes_released" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="manga_updates_id" Member="manga_updates_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="year_released" Member="year_released" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="description" Member="description" Type="System.String" DbType="TEXT" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="Tags" Member="Tags">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="value" Member="value" Type="System.String" DbType="TEXT" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="PageTags" Member="PageTags">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="Pages_id" Member="Pages_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="Tags_id" Member="Tags_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="Pages" Member="Pages">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="Files_id" Member="Files_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="Volumes_id" Member="Volumes_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="Genres" Member="Genres">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="value" Member="value" Type="System.String" DbType="TEXT" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="MangaGenres" Member="MangaGenres">
    <Column Name="Id" Member="Id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="Manga_id" Member="Manga_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="Genres_id" Member="Genres_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="Categories" Member="Categories">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="value" Member="value" Type="System.String" DbType="TEXT" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="MangaCategories" Member="MangaCategories">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="Manga_id" Member="Manga_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="Categories_id" Member="Categories_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="People" Member="People">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="name" Member="name" Type="System.String" DbType="TEXT" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="MangaPeople" Member="MangaPeople">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="People_id" Member="People_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="Manga_id" Member="Manga_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="is_artist" Member="is_artist" Type="System.Decimal" DbType="NUMERIC" IsPrimaryKey="false" Nullable="false" />
    <Column Name="is_author" Member="is_author" Type="System.Decimal" DbType="NUMERIC" IsPrimaryKey="false" Nullable="false" />
  </Table>
  <Table Name="MangaTitles" Member="MangaTitles">
    <Column Name="id" Member="id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="true" Nullable="false" />
    <Column Name="Manga_id" Member="Manga_id" Type="System.Int32" DbType="INTEGER" IsPrimaryKey="false" Nullable="false" />
    <Column Name="value" Member="value" Type="System.String" DbType="TEXT" IsPrimaryKey="false" Nullable="false" />
    <Column Name="is_primary" Member="is_primary" Type="System.Decimal" DbType="NUMERIC" IsPrimaryKey="false" Nullable="false" />
    <Index Name="Test">
      <IndexColumn Name="Manga_id" />
    </Index>
  </Table>
</Database>