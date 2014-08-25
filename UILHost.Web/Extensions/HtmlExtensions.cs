using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using UILHost.Common;
using UILHost.Infrastructure.Domain;
using UILHost.Repository.Pattern.Ef6;
using UILHost.Repository.Pattern.UnitOfWork;
using UILHost.Web.Service;
using UILHost.Infrastructure.IoC;
using DependencyResolver = UILHost.Infrastructure.IoC.DependencyResolver;

namespace UILHost.Web.Extensions
{
    public static class HtmlExtensions
    {
        public static string GetCurrentUsername(this HtmlHelper html)
        {
            var authSvc = Infrastructure.IoC.DependencyResolver.GetDependency<IAuthenticationService>();

            return authSvc.GetCurrentUserFirstName();
        }

        //public static bool DoesCurrentUserHaveModuleAccess(this HtmlHelper html, SecurityModuleKey permissionKey)
        //{
        //    var svc = Infrastructure.IoC.DependencyResolver.GetDependency<IAuthenticationService>();

        //    return svc.CurrentUserProfileHasPermission(permissionKey);
        //}

        private static HtmlString GenerateSelectOptions<TEntity, TPropertyValue, TPropertyDescription>(this HtmlHelper html,
            IEnumerable<TEntity> entities,
            Expression<Func<TEntity, TPropertyValue>> valueProperty,
            Expression<Func<TEntity, TPropertyDescription>> descriptionProperty,
            bool doGenerateNullOption = true)
        {
            var valueInfo = ((MemberExpression) valueProperty.Body).Member as PropertyInfo;
            var descInfo = ((MemberExpression) descriptionProperty.Body).Member as PropertyInfo;

            var sb = new StringBuilder();
            if (doGenerateNullOption)
                sb.Append("<option value=\'-1\'> - Select - </option>");

            foreach (var obj in entities)
                sb.Append(string.Format("<option value=\'{0}\'>{1}</option>",
                    valueInfo.GetValue(obj),
                    descInfo.GetValue(obj)));

            return new HtmlString(sb.ToString());
        }

        public static HtmlString GenerateSelectOptions<TEntity, TValue, TText>(
            this HtmlHelper html,
            TEntity x,
            Expression<Func<TEntity, TValue>> value,
            Expression<Func<TEntity, TText>> text,
            bool doGenerateNullOption = false) where TEntity : Entity
        {
            var uow = DependencyResolver.GetDependency<IUnitOfWorkAsync>();
            var entities = uow.RepositoryAsync<TEntity>().Queryable();
            var valueInfo = ((MemberExpression)value.Body).Member as PropertyInfo;
            var descInfo = ((MemberExpression)text.Body).Member as PropertyInfo;

            var sb = new StringBuilder();
            if (doGenerateNullOption)
                sb.Append("<option value=\'0\'> - Select - </option>");

            foreach (var obj in entities)
                sb.Append(string.Format("<option value=\'{0}\'>{1}</option>",
                    valueInfo.GetValue(obj),
                    descInfo.GetValue(obj)));

            return new HtmlString(sb.ToString());
        }

        public static bool HasPermission(this HtmlHelper htmlHelper)
        {
            var teacher = DependencyResolver.GetDependency<IAuthenticationService>().GetCurrentTeacher();
            return teacher.UserProfilePermissionFlag == UserProfilePermissionFlag.Director
                   || teacher.UserProfilePermissionFlag == UserProfilePermissionFlag.All;
        }

        //public static HtmlString GenerateStateSelectOptions(this HtmlHelper html)
        //{
        //    return GenerateSelectOptions(
        //        Infrastructure.IoC.DependencyResolver.GetDependency<IStateService>().Read(),
        //        s => s.Id,
        //        s => s.Name);
        //}


        public static HtmlString DisplayOrEditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, bool isEditor)
        {
            return isEditor ? html.EditorFor(expression) : html.DisplayFor(expression);
        }

        public static HtmlString GenerateSelectListFromEnum<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression,
            object htmlAttributes = null,
            params Enum[] excludeEnum)
        {
            //make sure we have the expression

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }


            //Get model information

            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumList = Enum.GetValues(metadata.ModelType).Cast<Enum>().ToList();
            enumList.RemoveAll(excludeEnum.Contains);
            var model = enumList.Select(e => new
            {
                Value = Convert.ToInt64(e),
                Description = e.GetDescription()
            })
                .OrderBy(i => i.Value);


            //build select tag

            var fullList = new StringBuilder();

            var selectList = new TagBuilder("select");
            selectList.MergeAttribute("id", metadata.PropertyName);
            selectList.MergeAttribute("name", metadata.PropertyName);

            selectList.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
                //user values will overwrite ours


            //Generate option tags

            foreach (var item in model)
            {
                var option = new TagBuilder("option") {InnerHtml = item.Description};
                option.Attributes.Add("value", item.Value.ToString(CultureInfo.InvariantCulture));
                if (item.Description == metadata.Model.ToString())
                {
                    option.MergeAttribute("selected", "");
                }
                fullList.AppendLine(option.ToString());
            }

            //add options to selectors

            selectList.InnerHtml = fullList.ToString();

            return MvcHtmlString.Create(selectList.ToString());
        }

        /// <summary>
        /// Takes a list of Enums to create security tag for an element. Enums should be either SecurityPermissionKey or SecurityModuleKey; all others will be
        /// filtered out.
        /// </summary>
        /// <param name="html">this HtmlHelper</param>
        /// <param name="securityKeys">A set of Enums that will be used to generate the security field</param>
        /// <returns>
        /// A string of dot delemited items of the format "P#" or "M#", where 'P' represents a SecurityPermissionKey and 'M'
        /// represents a SecurityModuleKey. Example output: 'security="M1.P2.M54.P123"'
        /// </returns>
        //public static HtmlString SecureElement(this HtmlHelper html, params Enum[] securityKeys)
        //{
        //    var sb = new StringBuilder();

        //    foreach (var securityKey in securityKeys)
        //    {
        //        if (securityKey is SecurityPermissionKey)
        //        {
        //            sb.AppendFormat("P{0}.", Convert.ChangeType(securityKey, typeof(long)));
        //        }
        //        if (securityKey is SecurityModuleKey)
        //        {
        //            sb.AppendFormat("M{0}.", Convert.ChangeType(securityKey, typeof(long)));
        //        }
        //    }

        //    if (sb.Length > 0)
        //    {
        //        sb.Remove(sb.Length - 1, 1); //remove last delimeter
        //    }
        //    return new HtmlString(String.Format("security=\"{0}\"", sb));
        //}

        //public static HtmlString SecureElement<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TValue>> expression)
        //{
        //    //make sure we have the expression

        //    if (expression == null)
        //    {
        //        throw new ArgumentNullException("expression");
        //    }


        //    //Get model information

        //    var memberExpression = expression.Body as MemberExpression;

        //    if (memberExpression == null)
        //        throw new InvalidOperationException("Expression must be a member expression");

        //    var securityAttribute = memberExpression.Member.GetAttribute<SecurityAttribute>();

        //    return new HtmlString(securityAttribute == null ? "" : securityAttribute.GetSecurityString());
        //}

        private static T GetAttribute<T>(this ICustomAttributeProvider provider)
            where T : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof (T), true);
            return attributes.Length > 0 ? attributes[0] as T : null;
        }
    }
}