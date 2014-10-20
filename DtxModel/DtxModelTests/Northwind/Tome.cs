using System;
using System.Data.Common;
using System.Collections.Generic;
using DtxModel;

namespace DtxModelTests.Tome {

	public class TomeContext : Context {
		private static Func<DbConnection> _default_connection = null;

		/// <summary>
		/// Set a default constructor to allow use of parameterless context calling.
		/// </summary>
		public static Func<DbConnection> DefaultConnection {
			get { return _default_connection; }
			set { _default_connection = value; }
		}

		private Table<Volumes> _Volumes;

		public Table<Volumes> Volumes {
			get {
				if(_Volumes == null) {
					_Volumes = new Table<Volumes>(this);
				}

				return _Volumes;
			}
		}

		private Table<Manga> _Manga;

		public Table<Manga> Manga {
			get {
				if(_Manga == null) {
					_Manga = new Table<Manga>(this);
				}

				return _Manga;
			}
		}

		private Table<Tags> _Tags;

		public Table<Tags> Tags {
			get {
				if(_Tags == null) {
					_Tags = new Table<Tags>(this);
				}

				return _Tags;
			}
		}

		private Table<PageTags> _PageTags;

		public Table<PageTags> PageTags {
			get {
				if(_PageTags == null) {
					_PageTags = new Table<PageTags>(this);
				}

				return _PageTags;
			}
		}

		private Table<Pages> _Pages;

		public Table<Pages> Pages {
			get {
				if(_Pages == null) {
					_Pages = new Table<Pages>(this);
				}

				return _Pages;
			}
		}

		private Table<Genres> _Genres;

		public Table<Genres> Genres {
			get {
				if(_Genres == null) {
					_Genres = new Table<Genres>(this);
				}

				return _Genres;
			}
		}

		private Table<MangaGenres> _MangaGenres;

		public Table<MangaGenres> MangaGenres {
			get {
				if(_MangaGenres == null) {
					_MangaGenres = new Table<MangaGenres>(this);
				}

				return _MangaGenres;
			}
		}

		private Table<Categories> _Categories;

		public Table<Categories> Categories {
			get {
				if(_Categories == null) {
					_Categories = new Table<Categories>(this);
				}

				return _Categories;
			}
		}

		private Table<MangaCategories> _MangaCategories;

		public Table<MangaCategories> MangaCategories {
			get {
				if(_MangaCategories == null) {
					_MangaCategories = new Table<MangaCategories>(this);
				}

				return _MangaCategories;
			}
		}

		private Table<People> _People;

		public Table<People> People {
			get {
				if(_People == null) {
					_People = new Table<People>(this);
				}

				return _People;
			}
		}

		private Table<MangaPeople> _MangaPeople;

		public Table<MangaPeople> MangaPeople {
			get {
				if(_MangaPeople == null) {
					_MangaPeople = new Table<MangaPeople>(this);
				}

				return _MangaPeople;
			}
		}

		private Table<MangaTitles> _MangaTitles;

		public Table<MangaTitles> MangaTitles {
			get {
				if(_MangaTitles == null) {
					_MangaTitles = new Table<MangaTitles>(this);
				}

				return _MangaTitles;
			}
		}

		/// <summary>
		/// Create a new context of this database's type.  Can only be used if a default connection is specified.
		/// </summary>
		public TomeContext() : base(_default_connection) { }

		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public TomeContext(DbConnection connection) : base(connection) { }
	}

	[TableAttribute(Name = "Volumes")]
	public class Volumes : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _numberChanged = false;
		private System.Int32 _number;
		public System.Int32 number {
			get { return _number; }
			set {
				_number = value;
				_numberChanged = true;
			}
		}

		private bool _descriptionChanged = false;
		private System.String _description;
		public System.String description {
			get { return _description; }
			set {
				_description = value;
				_descriptionChanged = true;
			}
		}

		private bool _Manga_idChanged = false;
		private System.Int32 _Manga_id;
		public System.Int32 Manga_id {
			get { return _Manga_id; }
			set {
				_Manga_id = value;
				_Manga_idChanged = true;
			}
		}

		private Pages _Pages;
		public Pages Pages {
			get {
				if(_Pages == null){ 
					try {
						_Pages = ((TomeContext)context).Pages.Select().WhereIn("Volumes_id", _id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Pages = null;
					}
				}
				return _Pages;
			}
		}

		private Manga[] _Manga;
		public Manga[] Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.Select().WhereIn("id", _Manga_id).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public Volumes() : this(null, null) { }

		public Volumes(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "number": _number = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "description": _description = reader.GetValue(i) as System.String; break;
					case "Manga_id": _Manga_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_numberChanged)
				changed.Add("number", _number);
			if (_descriptionChanged)
				changed.Add("description", _description);
			if (_Manga_idChanged)
				changed.Add("Manga_id", _Manga_id);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_number,
				_description,
				_Manga_id,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"number",
				"description",
				"Manga_id",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Manga")]
	public class Manga : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _status_enumChanged = false;
		private System.Int32 _status_enum;
		public System.Int32 status_enum {
			get { return _status_enum; }
			set {
				_status_enum = value;
				_status_enumChanged = true;
			}
		}

		private bool _last_updatedChanged = false;
		private System.DateTime _last_updated;
		public System.DateTime last_updated {
			get { return _last_updated; }
			set {
				_last_updated = value;
				_last_updatedChanged = true;
			}
		}

		private bool _total_volumes_releasedChanged = false;
		private System.Int32 _total_volumes_released;
		public System.Int32 total_volumes_released {
			get { return _total_volumes_released; }
			set {
				_total_volumes_released = value;
				_total_volumes_releasedChanged = true;
			}
		}

		private bool _manga_updates_idChanged = false;
		private System.Int32 _manga_updates_id;
		public System.Int32 manga_updates_id {
			get { return _manga_updates_id; }
			set {
				_manga_updates_id = value;
				_manga_updates_idChanged = true;
			}
		}

		private bool _year_releasedChanged = false;
		private System.Int32 _year_released;
		public System.Int32 year_released {
			get { return _year_released; }
			set {
				_year_released = value;
				_year_releasedChanged = true;
			}
		}

		private bool _descriptionChanged = false;
		private System.String _description;
		public System.String description {
			get { return _description; }
			set {
				_description = value;
				_descriptionChanged = true;
			}
		}

		private Volumes _VolumesModels;
		public Volumes VolumesModels {
			get {
				if(_VolumesModels == null){ 
					try {
						_VolumesModels = ((TomeContext)context).Volumes.Select().WhereIn("Manga_id", _id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_VolumesModels = null;
					}
				}
				return _VolumesModels;
			}
		}

		private MangaGenres _MangaGenres;
		public MangaGenres MangaGenres {
			get {
				if(_MangaGenres == null){ 
					try {
						_MangaGenres = ((TomeContext)context).MangaGenres.Select().WhereIn("Manga_id", _id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaGenres = null;
					}
				}
				return _MangaGenres;
			}
		}

		private MangaCategories _MangaCategories;
		public MangaCategories MangaCategories {
			get {
				if(_MangaCategories == null){ 
					try {
						_MangaCategories = ((TomeContext)context).MangaCategories.Select().WhereIn("Manga_id", _id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaCategories = null;
					}
				}
				return _MangaCategories;
			}
		}

		private MangaPeople _MangaPeople;
		public MangaPeople MangaPeople {
			get {
				if(_MangaPeople == null){ 
					try {
						_MangaPeople = ((TomeContext)context).MangaPeople.Select().WhereIn("Manga_id", _id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaPeople = null;
					}
				}
				return _MangaPeople;
			}
		}

		private MangaTitles _MangaTitles;
		public MangaTitles MangaTitles {
			get {
				if(_MangaTitles == null){ 
					try {
						_MangaTitles = ((TomeContext)context).MangaTitles.Select().WhereIn("Manga_id", _id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaTitles = null;
					}
				}
				return _MangaTitles;
			}
		}

		public Manga() : this(null, null) { }

		public Manga(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "status_enum": _status_enum = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "last_updated": _last_updated = (reader.IsDBNull(i)) ? default(System.DateTime) : reader.GetDateTime(i); break;
					case "total_volumes_released": _total_volumes_released = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "manga_updates_id": _manga_updates_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "year_released": _year_released = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "description": _description = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_status_enumChanged)
				changed.Add("status_enum", _status_enum);
			if (_last_updatedChanged)
				changed.Add("last_updated", _last_updated);
			if (_total_volumes_releasedChanged)
				changed.Add("total_volumes_released", _total_volumes_released);
			if (_manga_updates_idChanged)
				changed.Add("manga_updates_id", _manga_updates_id);
			if (_year_releasedChanged)
				changed.Add("year_released", _year_released);
			if (_descriptionChanged)
				changed.Add("description", _description);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_status_enum,
				_last_updated,
				_total_volumes_released,
				_manga_updates_id,
				_year_released,
				_description,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"status_enum",
				"last_updated",
				"total_volumes_released",
				"manga_updates_id",
				"year_released",
				"description",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Tags")]
	public class Tags : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _valueChanged = false;
		private System.String _value;
		public System.String value {
			get { return _value; }
			set {
				_value = value;
				_valueChanged = true;
			}
		}

		private PageTags _PageTags;
		public PageTags PageTags {
			get {
				if(_PageTags == null){ 
					try {
						_PageTags = ((TomeContext)context).PageTags.Select().WhereIn("Tags_id", _id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_PageTags = null;
					}
				}
				return _PageTags;
			}
		}

		public Tags() : this(null, null) { }

		public Tags(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "value": _value = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_valueChanged)
				changed.Add("value", _value);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_value,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"value",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "PageTags")]
	public class PageTags : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _Pages_idChanged = false;
		private System.Int32 _Pages_id;
		public System.Int32 Pages_id {
			get { return _Pages_id; }
			set {
				_Pages_id = value;
				_Pages_idChanged = true;
			}
		}

		private bool _Tags_idChanged = false;
		private System.Int32 _Tags_id;
		public System.Int32 Tags_id {
			get { return _Tags_id; }
			set {
				_Tags_id = value;
				_Tags_idChanged = true;
			}
		}

		private Tags _Tags;
		public Tags Tags {
			get {
				if(_Tags == null){ 
					try {
						_Tags = ((TomeContext)context).Tags.Select().WhereIn("id", _Tags_id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Tags = null;
					}
				}
				return _Tags;
			}
		}

		private Pages[] _Pages;
		public Pages[] Pages {
			get {
				if(_Pages == null){ 
					try {
						_Pages = ((TomeContext)context).Pages.Select().WhereIn("id", _Pages_id).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Pages = null;
					}
				}
				return _Pages;
			}
		}

		public PageTags() : this(null, null) { }

		public PageTags(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Pages_id": _Pages_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Tags_id": _Tags_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Pages_idChanged)
				changed.Add("Pages_id", _Pages_id);
			if (_Tags_idChanged)
				changed.Add("Tags_id", _Tags_id);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Pages_id,
				_Tags_id,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Pages_id",
				"Tags_id",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Pages")]
	public class Pages : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _Files_idChanged = false;
		private System.Int32 _Files_id;
		public System.Int32 Files_id {
			get { return _Files_id; }
			set {
				_Files_id = value;
				_Files_idChanged = true;
			}
		}

		private bool _Volumes_idChanged = false;
		private System.Int32 _Volumes_id;
		public System.Int32 Volumes_id {
			get { return _Volumes_id; }
			set {
				_Volumes_id = value;
				_Volumes_idChanged = true;
			}
		}

		private PageTags _PageTags;
		public PageTags PageTags {
			get {
				if(_PageTags == null){ 
					try {
						_PageTags = ((TomeContext)context).PageTags.Select().WhereIn("Pages_id", _id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_PageTags = null;
					}
				}
				return _PageTags;
			}
		}

		private Volumes[] _Volume;
		public Volumes[] Volume {
			get {
				if(_Volume == null){ 
					try {
						_Volume = ((TomeContext)context).Volumes.Select().WhereIn("id", _Volumes_id).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Volume = null;
					}
				}
				return _Volume;
			}
		}

		public Pages() : this(null, null) { }

		public Pages(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Files_id": _Files_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Volumes_id": _Volumes_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Files_idChanged)
				changed.Add("Files_id", _Files_id);
			if (_Volumes_idChanged)
				changed.Add("Volumes_id", _Volumes_id);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Files_id,
				_Volumes_id,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Files_id",
				"Volumes_id",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "Genres")]
	public class Genres : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _valueChanged = false;
		private System.String _value;
		public System.String value {
			get { return _value; }
			set {
				_value = value;
				_valueChanged = true;
			}
		}

		private MangaGenres[] _MangaGenres;
		public MangaGenres[] MangaGenres {
			get {
				if(_MangaGenres == null){ 
					try {
						_MangaGenres = ((TomeContext)context).MangaGenres.Select().WhereIn("Genres_id", _id).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaGenres = null;
					}
				}
				return _MangaGenres;
			}
		}

		public Genres() : this(null, null) { }

		public Genres(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "value": _value = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_valueChanged)
				changed.Add("value", _value);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_value,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"value",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "MangaGenres")]
	public class MangaGenres : Model {
		private System.Int32 _Id;
		public System.Int32 Id {
			get { return _Id; }
		}

		private bool _Manga_idChanged = false;
		private System.Int32 _Manga_id;
		public System.Int32 Manga_id {
			get { return _Manga_id; }
			set {
				_Manga_id = value;
				_Manga_idChanged = true;
			}
		}

		private bool _Genres_idChanged = false;
		private System.Int32 _Genres_id;
		public System.Int32 Genres_id {
			get { return _Genres_id; }
			set {
				_Genres_id = value;
				_Genres_idChanged = true;
			}
		}

		private Genres _Genres;
		public Genres Genres {
			get {
				if(_Genres == null){ 
					try {
						_Genres = ((TomeContext)context).Genres.Select().WhereIn("id", _Genres_id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Genres = null;
					}
				}
				return _Genres;
			}
		}

		private Manga[] _Manga;
		public Manga[] Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.Select().WhereIn("id", _Manga_id).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public MangaGenres() : this(null, null) { }

		public MangaGenres(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "Id": _Id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Manga_id": _Manga_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Genres_id": _Genres_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Manga_idChanged)
				changed.Add("Manga_id", _Manga_id);
			if (_Genres_idChanged)
				changed.Add("Genres_id", _Genres_id);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Manga_id,
				_Genres_id,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Manga_id",
				"Genres_id",
			};
		}

		public override string GetPKName() {
			return "Id";
		}

		public override object GetPKValue() {
			return _Id;
		}

	}

	[TableAttribute(Name = "Categories")]
	public class Categories : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _valueChanged = false;
		private System.String _value;
		public System.String value {
			get { return _value; }
			set {
				_value = value;
				_valueChanged = true;
			}
		}

		private MangaCategories[] _MangaCategories;
		public MangaCategories[] MangaCategories {
			get {
				if(_MangaCategories == null){ 
					try {
						_MangaCategories = ((TomeContext)context).MangaCategories.Select().WhereIn("Categories_id", _id).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaCategories = null;
					}
				}
				return _MangaCategories;
			}
		}

		public Categories() : this(null, null) { }

		public Categories(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "value": _value = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_valueChanged)
				changed.Add("value", _value);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_value,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"value",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "MangaCategories")]
	public class MangaCategories : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _Manga_idChanged = false;
		private System.Int32 _Manga_id;
		public System.Int32 Manga_id {
			get { return _Manga_id; }
			set {
				_Manga_id = value;
				_Manga_idChanged = true;
			}
		}

		private bool _Categories_idChanged = false;
		private System.Int32 _Categories_id;
		public System.Int32 Categories_id {
			get { return _Categories_id; }
			set {
				_Categories_id = value;
				_Categories_idChanged = true;
			}
		}

		private Categories _Categories;
		public Categories Categories {
			get {
				if(_Categories == null){ 
					try {
						_Categories = ((TomeContext)context).Categories.Select().WhereIn("id", _Categories_id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Categories = null;
					}
				}
				return _Categories;
			}
		}

		private Manga[] _Manga;
		public Manga[] Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.Select().WhereIn("id", _Manga_id).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public MangaCategories() : this(null, null) { }

		public MangaCategories(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Manga_id": _Manga_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Categories_id": _Categories_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Manga_idChanged)
				changed.Add("Manga_id", _Manga_id);
			if (_Categories_idChanged)
				changed.Add("Categories_id", _Categories_id);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Manga_id,
				_Categories_id,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Manga_id",
				"Categories_id",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "People")]
	public class People : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _nameChanged = false;
		private System.String _name;
		public System.String name {
			get { return _name; }
			set {
				_name = value;
				_nameChanged = true;
			}
		}

		private MangaPeople[] _MangaPeople;
		public MangaPeople[] MangaPeople {
			get {
				if(_MangaPeople == null){ 
					try {
						_MangaPeople = ((TomeContext)context).MangaPeople.Select().WhereIn("People_id", _id).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaPeople = null;
					}
				}
				return _MangaPeople;
			}
		}

		public People() : this(null, null) { }

		public People(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "name": _name = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_nameChanged)
				changed.Add("name", _name);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_name,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"name",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "MangaPeople")]
	public class MangaPeople : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _People_idChanged = false;
		private System.Int32 _People_id;
		public System.Int32 People_id {
			get { return _People_id; }
			set {
				_People_id = value;
				_People_idChanged = true;
			}
		}

		private bool _Manga_idChanged = false;
		private System.Int32 _Manga_id;
		public System.Int32 Manga_id {
			get { return _Manga_id; }
			set {
				_Manga_id = value;
				_Manga_idChanged = true;
			}
		}

		private bool _is_artistChanged = false;
		private System.Boolean _is_artist;
		public System.Boolean is_artist {
			get { return _is_artist; }
			set {
				_is_artist = value;
				_is_artistChanged = true;
			}
		}

		private bool _is_authorChanged = false;
		private System.Boolean _is_author;
		public System.Boolean is_author {
			get { return _is_author; }
			set {
				_is_author = value;
				_is_authorChanged = true;
			}
		}

		private People _People;
		public People People {
			get {
				if(_People == null){ 
					try {
						_People = ((TomeContext)context).People.Select().WhereIn("id", _People_id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_People = null;
					}
				}
				return _People;
			}
		}

		private Manga[] _Manga;
		public Manga[] Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.Select().WhereIn("id", _Manga_id).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public MangaPeople() : this(null, null) { }

		public MangaPeople(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "People_id": _People_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Manga_id": _Manga_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "is_artist": _is_artist = (reader.IsDBNull(i)) ? default(System.Boolean) : reader.GetBoolean(i); break;
					case "is_author": _is_author = (reader.IsDBNull(i)) ? default(System.Boolean) : reader.GetBoolean(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_People_idChanged)
				changed.Add("People_id", _People_id);
			if (_Manga_idChanged)
				changed.Add("Manga_id", _Manga_id);
			if (_is_artistChanged)
				changed.Add("is_artist", _is_artist);
			if (_is_authorChanged)
				changed.Add("is_author", _is_author);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_People_id,
				_Manga_id,
				_is_artist,
				_is_author,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"People_id",
				"Manga_id",
				"is_artist",
				"is_author",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}

	[TableAttribute(Name = "MangaTitles")]
	public class MangaTitles : Model {
		private System.Int32 _id;
		public System.Int32 id {
			get { return _id; }
		}

		private bool _Manga_idChanged = false;
		private System.Int32 _Manga_id;
		public System.Int32 Manga_id {
			get { return _Manga_id; }
			set {
				_Manga_id = value;
				_Manga_idChanged = true;
			}
		}

		private bool _valueChanged = false;
		private System.String _value;
		public System.String value {
			get { return _value; }
			set {
				_value = value;
				_valueChanged = true;
			}
		}

		private bool _is_primaryChanged = false;
		private System.Boolean _is_primary;
		public System.Boolean is_primary {
			get { return _is_primary; }
			set {
				_is_primary = value;
				_is_primaryChanged = true;
			}
		}

		private Manga _Manga;
		public Manga Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.Select().WhereIn("id", _Manga_id).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public MangaTitles() : this(null, null) { }

		public MangaTitles(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "id": _id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Manga_id": _Manga_id = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "value": _value = reader.GetValue(i) as System.String; break;
					case "is_primary": _is_primary = (reader.IsDBNull(i)) ? default(System.Boolean) : reader.GetBoolean(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Manga_idChanged)
				changed.Add("Manga_id", _Manga_id);
			if (_valueChanged)
				changed.Add("value", _value);
			if (_is_primaryChanged)
				changed.Add("is_primary", _is_primary);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_Manga_id,
				_value,
				_is_primary,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"Manga_id",
				"value",
				"is_primary",
			};
		}

		public override string GetPKName() {
			return "id";
		}

		public override object GetPKValue() {
			return _id;
		}

	}
}