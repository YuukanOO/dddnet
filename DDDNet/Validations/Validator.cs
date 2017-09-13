using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DDDNet.Validations
{
    /// <summary>
    /// Implémente un mécanisme simple de validation de paramètres en envoyant une ValidationException
    /// en cas d'erreur contenant une liste de FieldException représentant les erreurs de validation.
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// Nom de la resource concernée
        /// </summary>
        public string Resource { get; private set; }
        /// <summary>
        /// Préfixe à utiliser quand ajout d'une erreur pour un champ. Surtout utilisé pour la validation de listes imbriquées
        /// </summary>
        public string Prefix { get; private set; }
        /// <summary>
        /// Liste des erreurs de validation pour les champs testés
        /// </summary>
        public List<FieldException> Errors { get; private set; }
        /// <summary>
        /// Permet de vérifier si le Validator possède actuellement des erreurs
        /// </summary>
        public bool HasError { get { return Errors.Any(); } }

        /// <summary>
        /// Construit un nouveau validateur pour la resource fournie
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="nestedPrefix">Préfixe à appliquer au nom des champs en erreur</param>
        public Validator(string resource, string nestedPrefix = null)
        {
            Prefix = string.IsNullOrEmpty(nestedPrefix) ? string.Empty : nestedPrefix + ".";

            Resource = resource;
            Errors = new List<FieldException>();
        }

        /// <summary>
        /// Instantie un Validator pour la resource fournie. Simple petit raccourci.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static Validator For<T>(T resource)
        {
            return For<T>();
        }

        /// <summary>
        /// Instantie un Validator pour le type demandé. Simple petit raccourci.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Validator For<T>()
        {
            return new Validator(typeof(T).Name);
        }

        /// <summary>
        /// Ajoute une erreur au Validator actuel
        /// </summary>
        /// <param name="field"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Validator AddError(string field, string code, object data = null)
        {
            return AddError(new FieldException(Resource, Prefix + field, code, data));
        }

        /// <summary>
        /// Ajoute une exception de validation pour un champ au Validator actuel
        /// </summary>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public Validator AddError(params FieldException[] exceptions)
        {
            Errors.AddRange(exceptions);

            return this;
        }

        /// <summary>
        /// Lève une exception si au moins une erreur est présente dans ce Validator
        /// </summary>
        /// <exception cref="ValidationException">En cas d'erreur lors de la validation lors de l'un ou de plusieurs champs</exception>
        public void Throw()
        {
            if (HasError)
            {
                throw new ValidationException(Errors.ToArray());
            }
        }
    }

    /// <summary>
    /// Classe d'extension qui possède tous les Validator fournis de base dans cette librairie.
    /// </summary>
    public static class ValidatorBuiltIn
    {
        private static readonly Regex _emailRegex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Vérifie qu'une chaîne n'est pas vide ou nulle
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Validator IsRequired(this Validator validator, string field, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                validator.AddError(field, nameof(IsRequired));
            }

            return validator;
        }

        /// <summary>
        /// Vérifie qu'un Guid n'est pas vide
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Validator IsRequired(this Validator validator, string field, Guid value)
        {
            if (value == Guid.Empty)
            {
                validator.AddError(field, nameof(IsRequired));
            }

            return validator;
        }

        /// <summary>
        /// Vérifie qu'un objet n'est pas nul
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Validator IsRequired<T>(this Validator validator, string field, T value) where T : class
        {
            if (value == null)
            {
                validator.AddError(field, nameof(IsRequired));
            }

            return validator;
        }

        /// <summary>
        /// Vérifie que 2 objets sont identiques
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="otherField"></param>
        /// <param name="otherValue"></param>
        /// <returns></returns>
        public static Validator AreEqual<T>(this Validator validator, string field, T value, string otherField, T otherValue)
        {
            if (!value.Equals(otherValue))
            {
                validator.AddError(field, nameof(AreEqual), otherField);
            }

            return validator;
        }

        /// <summary>
        /// Vérifie que la valeur renseignée est bien une adresse email valide selon la RFC 2822
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Validator IsEmail(this Validator validator, string field, string value)
        {
            if (!_emailRegex.IsMatch(value))
            {
                validator.AddError(field, nameof(IsEmail));
            }

            return validator;
        }

        /// <summary>
        /// Vérifie qu'une chaîne possède un minimum de caractères
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Validator HasMinimumLength(this Validator validator, string field, string value, int length)
        {
            if (value?.Length < length)
            {
                validator.AddError(field, nameof(HasMinimumLength), length);
            }

            return validator;
        }

        /// <summary>
        /// Vérifie qu'une chaîne possède un maximum de caractères
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Validator HasMaximumLength(this Validator validator, string field, string value, int length)
        {
            if (value?.Length > length)
            {
                validator.AddError(field, nameof(HasMaximumLength), length);
            }

            return validator;
        }

        /// <summary>
        /// Vérifie l'unicité d'un champ selon le prédicat fournit
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="trueIfUnique">Lambda à éxécuter, si vrai, alors la condition est validée, sinon une erreur est ajoutée</param>
        /// <returns></returns>
        public static Validator IsUnique(this Validator validator, string field, Func<bool> trueIfUnique)
        {
            if (!trueIfUnique())
            {
                validator.AddError(field, nameof(IsUnique));
            }

            return validator;
        }

        /// <summary>
        /// Applique des validations sur une collection d'objet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="collection"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static Validator Each<T>(this Validator validator, string field, IEnumerable<T> collection, Action<Validator, T> handler)
        {
            int i = 0;

            foreach(var item in collection)
            {
                var nestedValidator = new Validator(validator.Resource, $"{field}[{i}]");

                handler(nestedValidator, item);

                if (nestedValidator.HasError)
                {
                    validator.AddError(nestedValidator.Errors.ToArray());
                }

                ++i;
            }

            return validator;
        }

        /// <summary>
        /// Valide un champ imbriqué
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static Validator For<T>(this Validator validator, string field, T value, Action<Validator, T> handler)
        {
            var nestedValidator = new Validator(validator.Resource, field);

            handler(nestedValidator, value);

            if (nestedValidator.HasError)
            {
                validator.AddError(nestedValidator.Errors.ToArray());
            }

            return validator;
        }
    }
}
