package com.codeplex.citylizard;

import java.util.ArrayList;
import java.util.Arrays;
import org.w3c.dom.Document;

/**
 * An XML builder.
 * @author Sergey Shandar.
 */
public final class XmlBuilder {
    
    /**
     * An XML node.
     */
    public static interface Node {
        /**
         * The function adds the node to the passed element.
         * @param domParent an element.
         */
        void addToDomElement(org.w3c.dom.Element domParent);
    }
    
    /**
     * An XML global name.
     */
    public final static class Name {
        
        private final String namespace;
        
        private final String localName;
        
        /**
         * A constructor.
         * @param namespace an XML namespace.
         * @param localName a local name.
         */
        public Name(String namespace, String localName) {
            this.namespace = namespace;
            this.localName = localName;
        }
    }
    
    /**
     * An XML element.
     */
    public final static class Element implements Node {
        
        private final Name name;
        
        private final ArrayList<Node> nodes;
        
        private void addToDomNode(
            org.w3c.dom.Node domParent, 
            Document domDocument) 
        {
            final org.w3c.dom.Element domElement =
                domDocument.createElementNS(
                    this.name.namespace, 
                    this.name.localName);
            for(final Node node: this.nodes) {
                node.addToDomElement(domElement);                
            }
            domParent.appendChild(domElement);
        }
        
        /**
         * A constructor. 
         * @param name an XML name.
         * @param nodes nodes.
         */
        public Element(Name name, Node...nodes) {
            this.name = name;
            this.nodes = new ArrayList(Arrays.asList(nodes));
        }
        
        /**
         * The function adds the element to the DOM document.
         * @param domDocument the DOM document.
         */
        public void addToDomDocument(Document domDocument) {
            this.addToDomNode(domDocument, domDocument);
        }
        
        public Element add(Node node) {
            this.nodes.add(node);
            return this;
        }

        @Override
        public void addToDomElement(org.w3c.dom.Element domParent) {
            this.addToDomNode(domParent, domParent.getOwnerDocument());
        }
    }
    
    /**
     * An XML attribute.
     */
    public final static class Attribute implements Node {
        
        private final Name name;
        
        private final String value;
        
        /**
         *  A constructor.
         * @param name an XML name.
         * @param value a value.
         */
        public Attribute(Name name, String value) {
            this.name = name;
            this.value = value;
        }

        @Override
        public void addToDomElement(org.w3c.dom.Element domParent) {
            domParent.setAttributeNS(
                this.name.namespace,
                this.name.localName,
                this.value);
        }
    }
    
    /**
     * An XML text.
     */
    public final static class Text implements Node {
        
        private final String value;
        
        /**
         * The constructor creates a text node.
         * @param value the text.
         */
        public Text(String value) {
            this.value = value;
        }

        @Override
        public void addToDomElement(org.w3c.dom.Element domParent) {
            domParent.appendChild(
                domParent.getOwnerDocument().createTextNode(this.value));
        }
    }
    
    /** 
     * An XML namespace.
     */
    public final static class Namespace {
        
        private final String namespace;
        
        /**
         * The function creates an XML global name. 
         * @param localName an XML local name.
         * @return an XML global name.
         */
        private Name name(String localName) {
            return new Name(this.namespace, localName);
        }
        
        /**
         * A constructor.
         * @param namespace a namespace. 
         */
        public Namespace(String namespace) {
            this.namespace = namespace;
        }
        
        /**
         * The function creates an XML element.
         * @param localName an XML local name.
         * @param nodes XML nodes.
         * @return an XML element.
         */
        public Element e(String localName, Node...nodes) {
            return new Element(this.name(localName), nodes);
        }
        
        /**
         * The function creates an XML attribute.
         * @param localName an XML local name.
         * @param value a text.
         * @return an XML attribute.
         */
        public Attribute a(String localName, String value) {
            return new Attribute(this.name(localName), value);
        }
    }
    
    /**
     * The function returns XML text.
     * @param value the text.
     * @return XML text.
     */
    public static Text t(String value) {
        return new Text(value);
    }
    
    /**
     * The function returns an XML attribute.
     * @param name an attribute name.
     * @param value an attribute value.
     * @return 
     */
    public static Attribute a(String name, String value) {
        return new Attribute(new Name(null, name), value);
    }
}
